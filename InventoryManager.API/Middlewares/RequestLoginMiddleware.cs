using System.Text;

namespace InventoryManager.API.Middlewares
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation(
                $"Request Method: {context.Request.Method}, Path: {context.Request.Path}, QueryString: {context.Request.QueryString}, Headers: {string.Join(",", context.Request.Headers)}, Body: {await FormatRequest(context.Request)}");

            await _next(context);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
            var body = await reader.ReadToEndAsync();

            request.Body.Position = 0;

            return body;
        }
    }
}
