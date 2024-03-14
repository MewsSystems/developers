using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateService.Infrastructure;

public class ExceptionMiddleware(RequestDelegate next)
{
    private static readonly Action<ILogger<ExceptionMiddleware>, string, string, string, string, Exception?> LogValidationException =
        LoggerMessage.Define<string, string, string, string>(
            LogLevel.Information,
            new EventId(2),
            "{Method} {ApiUrl} did not started due to {ExceptionName}, {TraceIdentifier}.");
    
    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException exception)
        {
            ProblemDetails problem = new();
            problem.Title = "Validation Failed";
            problem.Status = StatusCodes.Status400BadRequest;
            problem.Extensions.Add("errors", exception.Errors.Select(x => new
            {
                x.PropertyName, x.ErrorMessage
            }));

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            LogValidationException(logger, context.Request.Method, context.Request.GetDisplayUrl(), nameof(ValidationException), context.TraceIdentifier, exception);
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}