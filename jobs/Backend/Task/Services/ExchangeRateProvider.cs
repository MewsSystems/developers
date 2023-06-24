using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Services
{
    public class ExchangeRateProvider
    {
        private readonly string _targetCurrencyCode;
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateProvider(
            string targetCurrencyCode,
            IExchangeRateService exchangeRateService)
        {
            _targetCurrencyCode = targetCurrencyCode;
            _exchangeRateService = exchangeRateService;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var exchangeRates = await _exchangeRateService.GetCurrencyExchangeRatesAsync(_targetCurrencyCode);

            return
                from currency in currencies
                join currencyExchangeRate in exchangeRates
                    on currency.Code equals currencyExchangeRate.SourceCurrency.Code into joined
                from presentCurrencyExchangeRate in joined
                select presentCurrencyExchangeRate;
        }
    }
}
