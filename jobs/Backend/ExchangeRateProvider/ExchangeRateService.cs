using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace ExchangeRateProvider
{
    public class ExchangeRateService
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return ExchangeRateCache.Index.Where(x => currencies.Contains(x.Key)).Select(x => x.Value).ToList();
        }


    }
}
