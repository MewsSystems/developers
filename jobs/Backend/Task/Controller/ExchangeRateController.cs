using ExchangeRateProvider;
using ExchangeRateProvider.Objects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateProviderService _exchangeRateProviderService;

        public ExchangeRateController(IExchangeRateProviderService exchangeRateProviderService)
        {
            _exchangeRateProviderService = exchangeRateProviderService;
        }

        /// <summary>
        /// Provides today exchange rates for the given currencies
        /// </summary>
        /// <param name="currencies">List of currencies in ISO</param>
        /// <returns></returns>
        [HttpPost("exchange-rates")]
        public async Task<string> GetExchangeRates([FromBody] List<Currency> currencies)
        {
            var exchangeRates = await _exchangeRateProviderService.RetrieveExchangeRatesAsync(currencies, DateTime.Today);
            return string.Join("\n", exchangeRates.Select(rate => rate.ToString()));
        }

        /// <summary>
        /// Provides exchange rates for the given currencies and date
        /// </summary>
        /// <param name="currencies">List of currencies</param>
        /// <param name="date">Exchange rate date in format yyyy-MM-dd</param>
        [HttpPost("exchange-rates-by-date")]
        public async Task<string> GetExchangeRatesByDate([FromBody] List<Currency> currencies, DateTime date)
        {
            var exchangeRates = await _exchangeRateProviderService.RetrieveExchangeRatesAsync(currencies, date);
            return string.Join("\n", exchangeRates.Select(rate => rate.ToString()));
        }
        /// <summary>
        /// Provides exchange rates for currencies: USD, EUR, CZK, JPY, KES, RUB, THB, TRY, XYZ. For today
        /// </summary>
        [HttpGet("test-exchange-rates")]
        public async Task<string> GetTestExchangeRates()
        {
            var exchangeRates = await _exchangeRateProviderService.RetrieveExchangeRatesAsync(new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
                new Currency("JPY"),
                new Currency("KES"),
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")
            }, DateTime.Today);
            return string.Join("\n", exchangeRates.Select(rate => rate.ToString()));
        }
    }
}
