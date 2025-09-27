using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Core.Common;

namespace ExchangeRateUpdater.Api.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger,
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred.");

        var response = context.Response;
        response.ContentType = "application/json";
        
        var apiResponse = new ApiResponse
        {
            Success = false,
            Message = "An error occurred while processing your request",
            Errors = new List<string>()
        };

        switch (exception)
        {
            case ExchangeRateProviderException:
                response.StatusCode = (int)HttpStatusCode.BadGateway;
                apiResponse.Message = "Exchange rate provider error";
                apiResponse.Errors.Add(exception.Message);
                break;

            case ArgumentException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                apiResponse.Message = "Invalid input";
                apiResponse.Errors.Add(exception.Message);
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                apiResponse.Errors.Add(_environment.IsDevelopment() 
                    ? exception.ToString() 
                    : "An unexpected error occurred. Please try again later.");
                break;
        }

        var result = JsonSerializer.Serialize(apiResponse);
        await response.WriteAsync(result);
    }
}
