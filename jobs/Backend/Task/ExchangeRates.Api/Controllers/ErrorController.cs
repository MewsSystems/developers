using ExchangeRates.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers;

public class ErrorController : ControllerBase
{
    [HttpGet]
    [Route("/error")]
    public IActionResult Error()
    {
        Exception exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        // Here you can add your Domain exceptions and relate it to HTTP codes.
        var (statusCode, message) = exception switch
        {
            IStatusCodeException ex => ((int)ex.StatusCode, ex.ErrorMessage),
            NotImplementedException => (StatusCodes.Status501NotImplemented, "The server does not support the functionality required to fulfill the request."),
            HttpRequestException => (StatusCodes.Status502BadGateway, "The server received an invalid response from an upstream server."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        return Problem(statusCode: statusCode, title: message);
    }
}
