using GS2_API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace GS2_API.Middleware.Authorization
{
    public class RolePermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public RolePermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            // 1) Se permite acesso anônimo, deixe passar
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            // 2) Se o endpoint NÃO exige autorização, não aplique checagens de role
            var requiresAuth = endpoint?.Metadata?.GetMetadata<IAuthorizeData>() != null;
            if (!requiresAuth)
            {
                await _next(context);
                return;
            }

            // 3) Agora sim: endpoint exige auth → verificar autenticação
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                await WriteUnauthorized(context, "Token ausente ou inválido.");
                return;
            }

            var perfilClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrWhiteSpace(perfilClaim))
            {
                await WriteUnauthorized(context, "Profile ausente no token.");
                return;
            }

            if (!HasPermission(context, perfilClaim))
            {
                await WriteForbidden(context, "Acesso não permitido para este perfil.");
                return;
            }

            await _next(context);
        }

        private bool HasPermission(HttpContext context, string perfil)
        {
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;

            if (!Enum.TryParse<TipoUsuarioEnum>(perfil, out var perfilEnum))
                return false;

            return perfilEnum switch
            {
                TipoUsuarioEnum.Administrador => true,
                TipoUsuarioEnum.Instrutor => path.StartsWith("/api/cursos") && !path.StartsWith("/api/usuarios"),
                TipoUsuarioEnum.Cliente => path.StartsWith("/api/meuscursos"),
                _ => false
            };
        }

        private static async Task WriteUnauthorized(HttpContext ctx, string msg)
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(new { error = msg }));
        }

        private static async Task WriteForbidden(HttpContext ctx, string msg)
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(new { error = msg }));
        }
    }

    public static class RolePermissionMiddlewareExtension
    {
        public static IApplicationBuilder UseRolePermission(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RolePermissionMiddleware>();
        }
    }
}
