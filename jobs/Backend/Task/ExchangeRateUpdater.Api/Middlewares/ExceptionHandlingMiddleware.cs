using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Abstractions.Exceptions;

namespace ExchangeRateUpdater.Api.Middlewares;

/// <summary>
/// Middleware for handling exceptions globally in the application.
/// </summary>
/// <param name="next"></param>
/// <param name="logger"></param>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate next = next ?? throw new ArgumentNullException(nameof(next));
    private readonly ILogger<ExceptionHandlingMiddleware> logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly Dictionary<Type, HttpStatusCode> exceptionStatusCodes = new()
    {
        { typeof(ArgumentException), HttpStatusCode.BadRequest },
        { typeof(ExchangeRateNotFoundException), HttpStatusCode.NotFound },
        { typeof(InvalidOperationException), HttpStatusCode.InternalServerError }
    };

    /// <summary>
    /// Invokes the middleware to handle exceptions.
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles the exception and writes an appropriate response.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = exceptionStatusCodes.GetValueOrDefault(exception.GetType(), HttpStatusCode.InternalServerError);

        logger.LogError(exception, "Exception handled in {path}: {ExceptionType} - {Message}", 
            context.Request.Path, exception.GetType().Name, exception.Message);

        var result = JsonSerializer.Serialize(new { error = exception.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
