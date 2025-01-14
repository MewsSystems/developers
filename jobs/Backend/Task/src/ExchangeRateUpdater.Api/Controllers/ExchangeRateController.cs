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
