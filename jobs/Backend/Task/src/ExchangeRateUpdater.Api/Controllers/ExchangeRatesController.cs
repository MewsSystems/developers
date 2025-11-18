using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.BusinessLogic;
using ExchangeRateUpdater.BusinessLogic.Models;

namespace ExchangeRateUpdater.Api.Controllers
{
    /// <summary>
    /// API controller for exchange rate operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateManager _exchangeRateManager;
        private readonly ILogger<ExchangeRatesController> _logger;

        /// <summary>
        /// Constructor for ExchangeRatesController.
        /// </summary>
        public ExchangeRatesController(IExchangeRateManager exchangeRateManager, ILogger<ExchangeRatesController> logger)
        {
            _exchangeRateManager = exchangeRateManager ?? throw new ArgumentNullException(nameof(exchangeRateManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets exchange rates for specified currency codes.
        /// </summary>
        /// <param name="currencies">Comma-separated list of currency codes (e.g., "USD,EUR,JPY")</param>
        /// <returns>List of exchange rates from source currency to CZK</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExchangeRateViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetExchangeRates([FromQuery] string? currencies = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(currencies))
                {
                    return BadRequest("Currencies parameter is required. Provide comma-separated currency codes.");
                }

                var currencyList = currencies
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(c => new Currency(c.ToUpperInvariant()))
                    .ToList();

                if (currencyList.Count == 0)
                {
                    return BadRequest("At least one currency code is required.");
                }

                // Always include CZK for consistency
                if (!currencyList.Any(c => c.Code == "CZK"))
                {
                    currencyList.Add(new Currency("CZK"));
                }

                var rates = _exchangeRateManager.GetExchangeRates(currencyList);

                var result = rates
                    .Select(r => new ExchangeRateViewModel
                    {
                        SourceCurrencyCode = r.SourceCurrency.Code,
                        TargetCurrencyCode = r.TargetCurrency.Code,
                        Rate = r.Value
                    })
                    .ToList();

                return Ok(result);
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error occurred while retrieving exchange rates for currencies: {Currencies}", currencies);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { error = "Exchange rate service is temporarily unavailable." });
            }
            catch (OperationCanceledException cancelEx)
            {
                _logger.LogWarning(cancelEx, "Exchange rate request was cancelled for currencies: {Currencies}", currencies);
                return StatusCode(StatusCodes.Status408RequestTimeout, new { error = "Request timeout while retrieving exchange rates." });
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Invalid argument provided for currencies: {Currencies}", currencies);
                return BadRequest(new { error = "Invalid currency data." });
            }
            catch (InvalidOperationException ioEx)
            {
                _logger.LogError(ioEx, "Invalid operation while retrieving exchange rates for currencies: {Currencies}", currencies);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Failed to retrieve exchange rates." });
            }
        }
    }
}
