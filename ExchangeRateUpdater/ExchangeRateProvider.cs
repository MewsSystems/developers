using ExchangeRateUpdater.RatesSource;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private IExchangeRatesSource source;

        public ExchangeRateProvider(IExchangeRatesSource source)
        {
            this.source = source;
        }

        public ExchangeRateProvider() : this(new ApiFixerSource())
        {

        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
                return Enumerable.Empty<ExchangeRate>();

            return GetRatesFromSourceAsync(currencies).Result;
        }

        private async Task<IEnumerable<ExchangeRate>> GetRatesFromSourceAsync(IEnumerable<Currency> currencies)
        {
            ConcurrentBag<ExchangeRate> result = new ConcurrentBag<ExchangeRate>();
            foreach (var c in currencies)
            {
                var subresult = await source.GetLatestRatesAsync(c, currencies);
                foreach (var r in subresult)
                {
                    result.Add(r);
                }
            }

            return result;
        }
    }
}
