namespace EchangeRateUpdater.Api;

using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unexpected error occurred while processing the request.");

        var responseStatusCode = StatusCodes.Status500InternalServerError;
        var problemDetails = new ProblemDetails
        {
            Type = "internal_server_error",
            Title = "An unexpected error occurred",
            Instance = httpContext.Request.Path
        };

        if (exception is ValidationException validationException)
        {
            responseStatusCode = StatusCodes.Status400BadRequest;
            var errors = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => JsonNamingPolicy.CamelCase.ConvertName(g.Key),
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );

            problemDetails.Type = "validation_error";

            if (errors.Count > 0)
            {
                problemDetails.Title = "One or more validation errors occurred";
                problemDetails.Extensions.Add("errors", errors);
            }
            else
            {
                problemDetails.Title = validationException.Message;
            }
        }

        httpContext.Response.StatusCode = responseStatusCode;
        problemDetails.Status = responseStatusCode;
        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        };

        await problemDetailsService.WriteAsync(context);
        return true;
    }
}