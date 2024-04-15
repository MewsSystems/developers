using Asp.Versioning;
using ExchangeRateUpdater.Application.Common.Dto;
using ExchangeRateUpdater.Application.ExchangeRates.Dto;
using ExchangeRateUpdater.Application.ExchangeRates.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Controllers
{
    /// <summary>
    /// Represents a controller for managing exchange rates.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/exchange-rates")]
    public class ExchangeRatesController : ApiBaseController
    {
        /// <summary>
        /// Retrieves exchange rates based on the specified query parameters.
        /// </summary>
        /// <param name="query">The query parameters for retrieving exchange rates.</param>
        /// <returns>An action result containing the list of exchange rates.</returns>
        [HttpGet]
        public async Task<ActionResult<ListResponse<ExchangeRateDto>>> GetExchangeRates([FromQuery] GetExchangeRatesQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
