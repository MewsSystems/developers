using ExchangeRateUpdater.API.Models;
using ExchangeRateUpdater.Application.Queries;
using ExchangeRateUpdater.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.API.Controllers;

/// <summary>
/// Handles exchange rate retrieval requests.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json", "application/xml")]
public sealed class ExchangeRateController : ControllerBase
{
    private readonly ISender _sender;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeRateController"/> class.
    /// </summary>
    /// <param name="sender">The mediator sender used for handling requests.</param>
    public ExchangeRateController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Retrieves exchange rates for a given date and optional currencies.
    /// </summary>
    /// <param name="query">Query parameters including date (optional) and currencies (optional).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// /// <returns>
    /// An <see cref="ExchangeRateResponse"/> containing exchange rates for the given date.
    /// </returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get exchange rates by date and currencies.",
        Description = "Retrieve exchange rates for a given date. Defaults to today if no date is provided. If no currencies are specified, all available ones are returned."
    )]
    [ProducesResponseType(typeof(ExchangeRateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ExchangeRateResponse>> GetExchangeRates(
        [FromQuery] GetExchangeRatesQuery query,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }
}
