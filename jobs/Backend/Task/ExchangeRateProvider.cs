using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            using var httpClient = new HttpClient();

            var rates = _cache.GetCachedValues();
            var targetCurrency = Currency.DefaultCZK();

            foreach ( var currency in currencies ) 
            {
                var rate = rates.FirstOrDefault(x => x.currencyCode == currency.Code);
                if (rate == null) continue;
                yield return
                    new ExchangeRate(currency, targetCurrency, rate.rate);
            }
        }

        //public Task<IEnumerable<int>> GetAsync(IEnumerable<Currency> currencies)
        //{
        //    yield return Task.FromResult(1);
        //    yield return Task.FromResult(2);
        //}

    }
}
