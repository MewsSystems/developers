using System.Net;

namespace ExchangeRateUpdater.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (TaskCanceledException)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
            await httpContext.Response.WriteAsync("Request has timed out");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled error ocurred");
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsync("Something went wrong");
        }
    }
}
