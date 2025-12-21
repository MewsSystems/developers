using System.Diagnostics;

namespace ExchangeRateProvider.Api.Middleware;

/// <summary>
/// Middleware for logging HTTP requests and responses with timing information.
/// </summary>
public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

        // Log request
        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["RequestId"] = requestId,
            ["Method"] = context.Request.Method,
            ["Path"] = context.Request.Path,
            ["QueryString"] = context.Request.QueryString.ToString(),
            ["UserAgent"] = context.Request.Headers.UserAgent.ToString()
        }))
        {
            logger.LogInformation("HTTP {Method} {Path}{QueryString} started", 
                context.Request.Method, 
                context.Request.Path, 
                context.Request.QueryString);

            try
            {
                await next(context);
                
                stopwatch.Stop();
                
                logger.LogInformation("HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                
                logger.LogError(ex, "HTTP {Method} {Path} failed after {ElapsedMilliseconds}ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds);
                
                throw;
            }
        }
    }
}