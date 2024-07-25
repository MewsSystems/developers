using API.Models;
using API.Factory;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ExchangeRateProviderFactory _exchangeRateProviderFactory;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ExchangeRateController(ExchangeRateProviderFactory exchangeRateProviderFactory)
        {
            _exchangeRateProviderFactory = exchangeRateProviderFactory;
        }

        [HttpGet("{providerIdentifier}")]
        [ProducesResponseType(typeof(IEnumerable<ExchangeRate>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(499)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetExchangeRates(string providerIdentifier, [FromQuery] string currencies, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(providerIdentifier))
            {
                _logger.Warn("Provider identifier is required.");
                return BadRequest("Provider identifier is required.");
            }

            if (string.IsNullOrWhiteSpace(currencies))
            {
                _logger.Warn("Currencies parameter is required.");
                return BadRequest("Currencies parameter is required.");
            }

            try
            {
                var provider = _exchangeRateProviderFactory.GetExchangeRateProvider(providerIdentifier);
                if (provider is null)
                {
                    _logger.Warn("Failed to get Exchange Rate Provider {providerIdentifier}", providerIdentifier);
                    return StatusCode(404, $"Failed to get Exchange Rate Provider {providerIdentifier}");
                }

                var currencyCodes = currencies.Split(',')
                                               .Select(code => code.Trim())
                                               .Where(code => !string.IsNullOrWhiteSpace(code))
                                               .ToList();


                var currencyObjects = currencyCodes.Select(code => new Currency(code)).ToList();
                var rates = await provider.GetCurrentExchangeRatesAsync(currencyObjects, cancellationToken);

                return Ok(rates);
            }
            catch (OperationCanceledException ex)
            {
                _logger.Warn(ex, "Request was canceled.");
                return StatusCode(499, "Client closed request.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while getting exchange rates.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}