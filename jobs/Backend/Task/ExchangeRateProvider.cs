using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRateCache _cache;

        public ExchangeRateProvider(IExchangeRateCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var exchangeRates = new List<ExchangeRate>();
            using var httpClient = new HttpClient();

            var rates = await _cache.GetCachedValuesAsync();
            var targetCurrency = Currency.DefaultCZK();

            foreach ( var currency in currencies ) 
            {
                var rate = rates.FirstOrDefault(x => x.currencyCode == currency.Code);
                if (rate == null) continue;
                exchangeRates.Add(new ExchangeRate(currency, targetCurrency, rate.rate));
            }

            return exchangeRates;
        }

    }
}
