using ExchangeRateUpdater;
using ExchangeRateUpdaterAPI.Services.ExchangeRateProviderService;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdaterAPI.Controllers
{
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateController(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            IEnumerable<ExchangeRate> exchangeRates = Enumerable.Empty<ExchangeRate>();

            // TODO Add error handling
            exchangeRates = await _exchangeRateProvider.GetExchangeRates(currencies);

            return Ok(exchangeRates);
        }
    }
}

