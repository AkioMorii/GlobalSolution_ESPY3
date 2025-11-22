
using GS2_APP.Services;

namespace GS2_APP.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<CursoService>();
            services.AddScoped<LoginService>();
            services.AddScoped<TrilhaAprendizagemService>();
            services.AddScoped<PalavraChaveService>();
            services.AddScoped<NivelService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<CursoService>();
            services.AddScoped<ApiClientService>();
            services.AddScoped<TipoConteudoService>();
            //services.AddScoped<LoginService>();
            //services.AddScoped<LoginService>();
            //services.AddScoped<LoginService>();
            //services.AddScoped<LoginService>();
            //services.AddScoped<ConteudoService>();
            //services.AddScoped<CursoPalavraChaveService>();


            return services;
        }
    }
}
