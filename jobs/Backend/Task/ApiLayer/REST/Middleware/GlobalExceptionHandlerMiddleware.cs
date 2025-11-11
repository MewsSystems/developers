using System.Net;
using System.Text.Json;
using REST.Response.Models.Common;

namespace REST.Middleware;

/// <summary>
/// Middleware for handling unhandled exceptions globally.
/// Converts exceptions to consistent ApiResponse format.
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
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
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            // ArgumentNullException must come before ArgumentException (more specific first)
            ArgumentNullException nullEx => CreateResponse(
                HttpStatusCode.BadRequest,
                "Missing required field",
                nullEx.Message
            ),

            ArgumentException argEx => CreateResponse(
                HttpStatusCode.BadRequest,
                "Invalid argument",
                argEx.Message
            ),

            InvalidOperationException invEx => CreateResponse(
                HttpStatusCode.BadRequest,
                "Invalid operation",
                invEx.Message
            ),

            UnauthorizedAccessException => CreateResponse(
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                "You are not authorized to perform this action"
            ),

            KeyNotFoundException notFoundEx => CreateResponse(
                HttpStatusCode.NotFound,
                "Resource not found",
                notFoundEx.Message
            ),

            _ => CreateResponse(
                HttpStatusCode.InternalServerError,
                "Internal server error",
                _environment.IsDevelopment()
                    ? $"{exception.Message}\n{exception.StackTrace}"
                    : "An error occurred while processing your request. Please try again later."
            )
        };

        context.Response.StatusCode = response.StatusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }

    private static ApiResponse CreateResponse(HttpStatusCode statusCode, string message, string? detail = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Error = detail != null ? new ErrorResponse { Message = detail } : null,
            StatusCode = (int)statusCode
        };
    }
}

/// <summary>
/// Extension methods for registering the global exception handler middleware.
/// </summary>
public static class GlobalExceptionHandlerMiddlewareExtensions
{
    /// <summary>
    /// Adds the global exception handler middleware to the application pipeline.
    /// Should be registered early in the pipeline to catch all exceptions.
    /// </summary>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
