using ExchangeRateUpdater.Model;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ICnbApiClient cnbApiClient;

        public ExchangeRateProvider(ICnbApiClient cnbApiClient)
        {
            this.cnbApiClient = cnbApiClient;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken token)
        {

            IEnumerable<ExchangeRate> rates = await cnbApiClient.GetDailyRates(token).ConfigureAwait(false);

            var currenciesSet = currencies.ToHashSet();

            return rates.Where(rate => rate.IsSpecifiedFor(currenciesSet));
        }
    }
}
