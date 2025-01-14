using ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates;
using ExchangeRateUpdater.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers
{
    [ApiController]
    [Route("api/v1/exchange-rates")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExchangeRateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns last valid data for exchange rates among the specified currencies that are defined by the provider (by default Czech National Bank)
        /// </summary>
        /// <remarks>
        /// Regarding parameters, the currencies must be indicated, and the date optionally.
        /// 
        /// Currencies are in three-letter ISO 4217 code.
        /// Example: AUD for Australian Dollar
        /// 
        /// Date in ISO format (yyyy-MM-dd); default value: NOW
        /// Example : 2019-05-17
        /// </remarks>
        /// <param name="query">The currencies must be indicated, and the date optionally.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(typeof(IEnumerable<ExchangeRate>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExchangeRates(
            [FromQuery] GetExchangeRatesQuery query,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(query, cancellationToken);

            return Ok(response.ToList());
        }
    }
}
