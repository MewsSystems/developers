using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using FluentValidation.Results;

namespace Common.Middlewares
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "Internal Server Error";

            IEnumerable<ValidationFailure> errors;

            if (e.GetType() == typeof(ValidationException))
            {
                message = e.Message;
                errors=((ValidationException)e).Errors;

                return httpContext.Response.WriteAsync(new ValidationErrorDetails
                {
                    StatusCode = 400,
                    Message = message,
                    Errors=errors
                }.ToString());
            }

            return httpContext.Response.WriteAsync(new ExceptionErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
