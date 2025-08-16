using ExchangeRateProvider.Contract.Models;
using ExchangeRateProvider.Exceptions;
using ExchangeRateProvider.Providers;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateProvider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ILogger<ExchangeRateController> _logger;
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateController(ILogger<ExchangeRateController> logger, IExchangeRateProvider exchangeRateProvider)
        {
            _logger = logger;
            _exchangeRateProvider = exchangeRateProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Get(IEnumerable<Currency> currencies)
        {
            try
            {
                return Ok(await _exchangeRateProvider.GetExchangeRatesAsync(currencies));
            }
            catch (BaseApiUnavailableException ex)
            {
                _logger.LogError(ex, "National bank exchange rate api is unavailable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured in National bank exchange rate api response");
            }

            return BadRequest();
        }
    }
}
