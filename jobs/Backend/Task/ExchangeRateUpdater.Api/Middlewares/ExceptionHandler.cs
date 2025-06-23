using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Middlewares;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<ExceptionHandler>();
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = new ProblemDetails
        {
            Type = exception.GetType().ToString(),
            Status = (int)GetStatusCode(exception),
            Detail = exception.Message
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = problemDetails.Status.Value;
        var payload = JsonSerializer.Serialize(problemDetails);
        if (context.Response.StatusCode.Equals((int)HttpStatusCode.InternalServerError))
        {
            _logger.LogError(exception, "{Origin}, Exception: {Payload}", nameof(ExceptionHandler), payload);
        }
        await context.Response.WriteAsync(payload);
    }

    private static HttpStatusCode GetStatusCode(Exception exception)
    {
        return exception switch
        {
            CustomValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
    }
}