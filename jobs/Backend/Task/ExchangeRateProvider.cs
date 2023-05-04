using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRatesListingsCache _ExchangeRatesListingCache;

        public ExchangeRateProvider(IExchangeRatesListingsCache exchangeRatesListingCache) {
            _ExchangeRatesListingCache = exchangeRatesListingCache;
        }

        /// <summary>
        /// Returns exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, not calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// the exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD" is not returned. If the source does not provide
        /// some of the requested currencies, they are ignored.
        /// </summary>
        /// <param name="currencies">Requested currencies.</param>
        /// <returns>Collection of exchange rates among requested currencies.</returns>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies) {
            var exchangeRates = await _ExchangeRatesListingCache.GetCurrentExchangeRates();

            return exchangeRates.Where(x => currencies.Contains(x.SourceCurrency) && currencies.Contains(x.TargetCurrency));
        }
    }
}
