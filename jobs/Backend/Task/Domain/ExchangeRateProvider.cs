using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain
{
    internal sealed class ExchangeRateProvider(IExchangeRateFetcher exchangeRateFetcher)
    {
        private readonly IExchangeRateFetcher exchangeRateFetcher = exchangeRateFetcher;

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currenciesSet = new HashSet<Currency>(currencies);

            var rates = await exchangeRateFetcher.GetExchangeRates();
            return rates.Where(r => currenciesSet.Contains(r.TargetCurrency));
        }
    }
}