using ExchangeRateUpdater.Api.Domain.Common;
using ExchangeRateUpdater.Api.Presentation.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Presentation.Controllers;

/// <summary>
/// Base controller providing shared error-to-action-result mapping using the ApiResponse pattern.
/// </summary>
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ToErrorResponse<T>(Error error)
    {
        var response = ApiResponse<T>.Failure(error.Code, error.Message);

        return error.Type switch
        {
            ErrorType.Validation => BadRequest(response),
            ErrorType.NotFound => NotFound(response),
            ErrorType.Unavailable => StatusCode(StatusCodes.Status502BadGateway, response),
            _ => StatusCode(StatusCodes.Status500InternalServerError, response)
        };
    }
}
