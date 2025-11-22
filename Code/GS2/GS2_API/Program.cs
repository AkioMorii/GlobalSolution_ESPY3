using AutoMapper;
using GS2_API.Auth;
using GS2_API.Extensions;
using GS2_API.Mappings;
using GS2_API.Middleware;
using GS2_API.Middleware.ApiVersion;
using GS2_API.Middleware.Authorization;
using GS2_API.Middleware.ErrorHandling;
using GS2_API.Services.v1;
using GS2_Domain;
using GS2_Infrastructure.Data;
using GS2_Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================================
// 1) AutoMapper
// ================================
builder.Services.AddAutoMapper(cfg =>
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

// Controllers / Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GS2 API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT neste campo usando: Bearer {seu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    // Adiciona servidores para HTTP e HTTPS
    ////c.AddServer(new OpenApiServer { Url = "http://localhost:7131" });
    //c.AddServer(new OpenApiServer { Url = "https://localhost:7130" });
});

// ================================
// 2) DbContext
// ================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================================
// 3) Repositórios / Services
// ================================
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<RefreshTokenRepository>();
builder.Services.AddScoped<ConteudoRepository>();
builder.Services.AddScoped<CursoPalavraChaveRepository>();
builder.Services.AddScoped<CursoRepository>();
builder.Services.AddBusinessServices();

// ================================
// 4) Configurações JWT
// ================================
var jwtSectionName = "JwtSettings";
var jwtSection = builder.Configuration.GetSection(jwtSectionName);

builder.Services.Configure<JwtSettings>(jwtSection);

var jwt = jwtSection.Get<JwtSettings>();
if (jwt == null)
    throw new InvalidOperationException($"Seção de configuração '{jwtSectionName}' não encontrada.");
if (string.IsNullOrWhiteSpace(jwt.Secret))
    throw new InvalidOperationException("JwtSettings: 'Secret' não configurado.");

var keyBytes = Encoding.UTF8.GetBytes(jwt.Secret);

// ================================
// 5) Autenticação JWT
// ================================
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

            ClockSkew = TimeSpan.Zero,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();
//builder.WebHost.UseUrls(
//    "http://localhost:7131",
//    "https://localhost:7130",
//    "http://0.0.0.0:7131"
//);
var app = builder.Build();

// ================================
// 6) Middlewares
// ================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseMiddleware<ApiVersionMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseMiddleware<JwtRefreshMiddleware>();
app.UseAuthorization();

// Controllers
app.MapControllers();

// ================================
// 7) RolePermission depois dos endpoints
// ================================
app.UseRolePermission();

// ================================
// 8) Versionamento
// ================================
app.Use(async (context, next) =>
{
    var version = context.Request.Headers["api-version"].FirstOrDefault() ?? "1.0";
    context.Items["api-version"] = version;
    await next();
});

app.Run();
