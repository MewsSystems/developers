using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Middleware
{
    /// <summary>
    /// Middleware that handles exceptions thrown during the processing of HTTP requests, logging the error and returning a
    /// standardized JSON response.
    /// </summary>
    /// <param name="next">The delegate that represents the next middleware in the request processing pipeline.</param>
    /// <param name="logger">The logger used to log unhandled exceptions that occur during request processing.</param>
    /// <param name="env">The web host environment that provides information about the hosting environment, such as whether the application is
    /// in development mode.</param>
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
    {
        private readonly Dictionary<Type, HttpStatusCode> exceptionStatusCodes = new()
        {
            { typeof(ArgumentException), HttpStatusCode.BadRequest },
            { typeof(ArgumentNullException), HttpStatusCode.BadRequest },
            { typeof(InvalidOperationException), HttpStatusCode.InternalServerError  }
        };

        /// <summary>
        /// Invokes the next middleware in the HTTP request pipeline asynchronously and handles any exceptions that
        /// occur during execution.
        /// </summary>
        /// <param name="context">The HttpContext for the current request, which encapsulates all HTTP-specific information about an
        /// individual HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation of invoking the next middleware and handling exceptions.</returns>
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
        /// Handles exceptions that occur during the processing of an HTTP request by logging the error and generating a
        /// standardized JSON error response.
        /// </summary>
        /// <param name="context">The HttpContext for the current HTTP request, used to set the response status code and write the error
        /// response.</param>
        /// <param name="ex">The exception that was thrown during request processing.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";
            var status = (int)exceptionStatusCodes.GetValueOrDefault(ex.GetType(), HttpStatusCode.InternalServerError);
            context.Response.StatusCode = status;

            var problem = new ProblemDetails
            {
                Status = status,
                Title = status == 500 ? "An internal error occurred." : "A request error occurred.",
                Detail = env.IsDevelopment() ? ex.ToString() : null
            };

            var json = JsonSerializer.Serialize(problem);
            await context.Response.WriteAsync(json);
        }
    }
}
