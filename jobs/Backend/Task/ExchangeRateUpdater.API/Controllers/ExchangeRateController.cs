using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly ILogger<ExchangeRateController> _logger;

        public ExchangeRateController(
            IExchangeRateProvider exchangeRateProvider,
            ILogger<ExchangeRateController> logger)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _logger = logger;
        }

        /// <summary>
        /// Gets exchange rates for the specified currencies or all exchange rates if no currencies are specified.
        /// </summary>
        /// <param name="currencies">A list of currency codes to get exchange rates for. If not specified, all exchange rates are returned.</param>
        /// <returns>A list of exchange rates.</returns>
        /// <response code="200">Returns the list of exchange rates.</response>
        /// <response code="204">No exchange rates available for the specified currencies.</response>
        /// <response code="503">Error occurred while fetching exchange rates from the external service.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public IActionResult Get([FromQuery] List<string>? currencies)
        {
            IEnumerable<ExchangeRate> result;
            try
            {
                if (currencies == null || currencies.Count == 0)
                {
                    _logger.LogInformation("No currencies provided, fetching all exchange rates.");
                    currencies = new List<string>();
                }
                else
                {
                    _logger.LogInformation("Fetching exchange rates for specified currencies.");
                }

                var currencyObjects = currencies.Select(c => new Currency(c)).ToList();
                result = _exchangeRateProvider.GetExchangeRates(currencyObjects);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while fetching exchange rates from the external service.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Error occurred while fetching exchange rates from the external service.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

            if (!result.Any())
            {
                return NoContent();
            }

            return Ok(result.Select(x => x.ToString()));
        }
    }
}