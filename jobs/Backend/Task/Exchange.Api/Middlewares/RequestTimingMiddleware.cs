using System.Diagnostics;

namespace Exchange.Api.Middlewares;

public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await next(context);

        stopwatch.Stop();

        var elapsedMs = stopwatch.ElapsedMilliseconds;
        var method = context.Request.Method;
        var path = context.Request.Path;
        var statusCode = context.Response.StatusCode;

        logger.LogInformation(
            "Request {Method} {Path} responded {StatusCode} in {Elapsed} ms",
            method,
            path,
            statusCode,
            elapsedMs
        );
    }
}