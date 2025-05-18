using System.Text.Json;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using FluentValidation;

namespace ExchangeRateUpdater.Api.Middlewares;

/// <summary>
/// Middleware for handling unhandled exceptions and converting them to appropriate HTTP responses
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline</param>
    /// <param name="logger">The logger instance for logging exceptions</param>
    /// <param name="environment">The hosting environment</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Processes the HTTP request and handles any unhandled exceptions
    /// </summary>
    /// <param name="context">The HTTP context for the request</param>
    /// <returns>A task that represents the asynchronous operation</returns>
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