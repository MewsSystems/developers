using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ExchangeRateUpdater.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, $"Unhandled exception {context.Request.Path}");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";

                var payload = new
                {
                    title = "An unexpected error occurred",
                    status = 500,
                    detail = ex.Message
                };

                var json = JsonSerializer.Serialize(payload);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
