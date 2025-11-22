using GS2_API.Auth;
using GS2_API.Services.v1;

namespace GS2_API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<LoginService>();
            services.AddScoped<TiposConteudoService>();
            services.AddScoped<TipoUsuarioService>();
            services.AddScoped<PalavraChaveService>();
            services.AddScoped<ConteudoService>();
            services.AddScoped<CursoPalavraChaveService>();
            services.AddScoped<NivelService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<CursoService>();
            services.AddScoped<TrilhaAprendizagemService>();
            //services.AddScoped<LoginService>();
            //services.AddScoped<LoginService>();
            //services.AddScoped<LoginService>();
            //services.AddScoped<LoginService>();

            services.AddScoped<TokenService>();
            services.AddScoped<RefreshTokenService>();
            return services;
        }
    }
}
