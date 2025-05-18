using System.Text.Json;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using FluentValidation;

namespace ExchangeRateUpdater.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context,
        Exception exception)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var errorResponse = new ErrorResponse
        {
            Status = statusCode,
            Title = "Internal Server Error",
            Detail = "An unexpected error occurred."
        };

        // Map specific exceptions to status codes and messages
        switch (exception)
        {
            case ValidationException validationEx:
                statusCode = StatusCodes.Status400BadRequest;
                errorResponse.Status = statusCode;
                errorResponse.Title = "Validation Error";
                errorResponse.Detail = "Validation failed.";
                errorResponse.Errors = validationEx.Errors.GroupBy(e => e.PropertyName).
                    ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).
                            ToArray()
                    );
                _logger.LogWarning(exception, "Validation error occurred");
                break;

            case ArgumentException argEx:
                statusCode = StatusCodes.Status400BadRequest;
                errorResponse.Status = statusCode;
                errorResponse.Title = "Bad Request";
                errorResponse.Detail = argEx.Message;
                _logger.LogWarning(exception, "Invalid argument: {Message}", argEx.Message);
                break;

            case InvalidOperationException invOpEx:
                statusCode = StatusCodes.Status404NotFound;
                errorResponse.Status = statusCode;
                errorResponse.Title = "Not Found";
                errorResponse.Detail = invOpEx.Message;
                _logger.LogWarning(exception, "Resource not found: {Message}", invOpEx.Message);
                break;

            default:
                _logger.LogError(exception, "An unhandled exception occurred");
                if (_environment.IsDevelopment()) errorResponse.Detail = exception.ToString();
                break;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
    }
}