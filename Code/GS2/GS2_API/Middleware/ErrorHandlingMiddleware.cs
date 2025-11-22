using System.Net;
using System.Text.Json;
using GS2_Domain.Exceptions;
using GS2_Domain.Exceptions.UserAccess;
namespace GS2_API.Middleware.ErrorHandling
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Erro não tratado capturado pelo middleware.");

            var response = context.Response;
            response.ContentType = "application/json";

            // ==== MAPEAR EXCEÇÕES CUSTOMIZADAS ====
            var (statusCode, message) = exception switch
            {
                // Exceptions de domínio (UserAccess)
                UsuarioInexistenteException e => (HttpStatusCode.NotFound, e.Message),
                CredenciaisInvalidasException e => (HttpStatusCode.Unauthorized, e.Message),
                UsuarioBloqueadoException e => (HttpStatusCode.Forbidden, e.Message),
                UsuarioNaoElegivelParaTrilhaException e => (HttpStatusCode.BadRequest, e.Message),


                TrilhaNaoEncontradaException e => (HttpStatusCode.NotFound, e.Message),
                // Outras exceções customizadas
                ArgumentException e => (HttpStatusCode.BadRequest, e.Message),
                KeyNotFoundException e => (HttpStatusCode.NotFound, e.Message),

                _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado.")
            };

            response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                success = false,
                error = message,
                status = statusCode.ToString()
            });

            await response.WriteAsync(result);
        }
    }
}
