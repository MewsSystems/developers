using ExchangeRateUpdater.ExchangeRateApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var client = new ExchangeRatesApiClient();
            var currencyPairs = new List<Tuple<Currency, Currency>>();

            // get pairs
            foreach (var baseCurrency in currencies)
            {
                var targets = currencies.Where(c => c.Code != baseCurrency.Code);
                currencyPairs.AddRange(targets.Select(t => new Tuple<Currency, Currency>(baseCurrency, t)));
            }

            // obtain rates without excessive thread blocking 
            var results = await Task.WhenAll(currencyPairs.Select(pair => client.GetExchangeRate(pair.Item1, pair.Item2)));
            return results.Where(rate => rate != null).ToList();
        }
    }
}
