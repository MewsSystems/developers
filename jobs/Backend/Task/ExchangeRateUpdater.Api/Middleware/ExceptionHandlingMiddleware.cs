using ExchangeRateUpdater.API.Models;
using ExchangeRateUpdater.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationException = ExchangeRateUpdater.Application.Exceptions.ApplicationException;

namespace ExchangeRateUpdater.API.Middleware;

/// <summary>
/// Middleware responsible for handling application-wide exceptions and returning consistent error responses.
/// </summary>
internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    /// <inheritdoc/>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
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
    /// Handles exceptions and generates a structured error response.
    /// </summary>
    /// <param name="httpContext">The HTTP context of the request.</param>
    /// <param name="exception">The exception that was thrown.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var response = new ApiErrorResponse
        {
            Title = GetTitle(exception),
            Status = statusCode,
            Detail = exception.Message,
            Errors = GetErrors(exception)
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    /// <summary>
    /// Maps exceptions to appropriate HTTP status codes.
    /// </summary>
    /// <param name="exception">The exception to evaluate.</param>
    /// <returns>The corresponding HTTP status code.</returns>
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => StatusCodes.Status412PreconditionFailed,
            NotFoundException => StatusCodes.Status404NotFound,
            ExternalServiceException => StatusCodes.Status502BadGateway,
            ParsingException => StatusCodes.Status500InternalServerError,
            CacheException => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError
        };

    /// <summary>
    /// Retrieves the error title based on the exception type.
    /// </summary>
    /// <param name="exception">The thrown exception.</param>
    /// <returns>A user-friendly error title.</returns>
    private static string GetTitle(Exception exception) =>
        exception switch
        {
            ApplicationException applicationException => applicationException.Title,
            _ => "Server Error"
        };

    /// <summary>
    /// Extracts validation errors from an exception, if applicable.
    /// </summary>
    /// <param name="exception">The thrown exception.</param>
    /// <returns>A dictionary of validation errors if available; otherwise, null.</returns>
    private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception) =>
        exception is ValidationException validationEx
            ? validationEx.ErrorsDictionary
            : null;
}
