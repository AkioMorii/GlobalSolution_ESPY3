namespace GS2_API.Middleware.ApiVersion
{
    public class ApiVersionMiddleware
    {
        private readonly RequestDelegate _next;
        private const string DefaultVersion = "1.0";

        public ApiVersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var version = context.Request.Headers["api-version"].FirstOrDefault() ?? DefaultVersion;
            context.Items["ApiVersion"] = version;
            await _next(context);
        }
    }
}

