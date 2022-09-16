using System.Collections.Generic;
using System.Linq;
using ExchangeRateProvider.Cache;
using Model;
using Model.Entities;

namespace ExchangeRateProvider.Service
{
    public class ExchangeRateService : IExchangeRateService
    {
        private ICache<Currency, ExchangeRate> _exchangeRateCache;
        public ExchangeRateService(ICache<Currency, ExchangeRate> exchangeRateCache)
        {
            _exchangeRateCache = exchangeRateCache;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return _exchangeRateCache.Index.Where(x => currencies.Contains(x.Key)).Select(x => x.Value).ToList();
        }
    }
}
