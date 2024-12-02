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
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get([FromQuery] List<string>? currencies)
        {
            IEnumerable<ExchangeRate> result;
            try
            {
                if (currencies == null)
                {
                    _logger.LogInformation("No currencies provided, fetching all exchange rates.");
                    currencies = new List<string>();
                }

                _logger.LogInformation("Fetching exchange rates for specified currencies.");
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

            return Ok(result.Select(x => x.ToString()));
        }
    }
}