using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Principal;

namespace ExchangeRateProvider.API
{
    public class GlobleExceptionHandler(ILogger<GlobleExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobleExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
            
            _logger.LogError(
                exception, 
                "Could not process request on Machine {Machine}. TraceId: {TraceId}",
                Environment.MachineName,
                traceId
            );

            var (statusCode, title) = MapExceptione(exception);

            await Results.Problem(
                title: title,
                statusCode: statusCode,
                extensions: new Dictionary<string, object?>
                {
                    { "TraceId", traceId }
                }
            ).ExecuteAsync(httpContext);
            
            return true;
        }

        private static (int StatusCode, string Title) MapExceptione(Exception exception)
        {
            return exception switch
            {
                ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "Oops server Error!")
            };
        }
    }
}
