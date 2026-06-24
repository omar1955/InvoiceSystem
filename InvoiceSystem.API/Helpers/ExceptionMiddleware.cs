using System.Net;
using System.Text.Json;

namespace InvoiceSystem.API.Helpers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = GetStatusCode(ex);

                var response = new
                {
                    success = false,
                    statusCode = context.Response.StatusCode,
                    message = ex.Message
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }

        private static int GetStatusCode(Exception ex)
        {
            if (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return (int)HttpStatusCode.NotFound;

            if (ex.Message.Contains("invalid email or password", StringComparison.OrdinalIgnoreCase))
                return (int)HttpStatusCode.Unauthorized;

            if (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                return (int)HttpStatusCode.BadRequest;

            return (int)HttpStatusCode.BadRequest;
        }
    }
}