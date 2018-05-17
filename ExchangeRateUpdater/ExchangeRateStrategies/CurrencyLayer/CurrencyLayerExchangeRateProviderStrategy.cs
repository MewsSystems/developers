using ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer
{
    public class CurrencyLayerExchangeRateProviderStrategy : IExchangeRateProviderSourceCurrencyStrategy
    {
        public const string ApiKey = "ac98137b1237bcb1a1bd2be2a4798eae";

        private readonly ICurrencyLayerRatesFetcher _fetcher;

        public CurrencyLayerExchangeRateProviderStrategy(ICurrencyLayerRatesFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(Currency sourceCurrency, IEnumerable<Currency> currencies)
        {
            var currenciesList = currencies as IList<Currency> ?? currencies.ToList();

            var response = await _fetcher.FetchRatesAsync(ApiKey, currenciesList.Select(currency => currency.Code));

            if (!sourceCurrency.Equals(new Currency(response.Source)))
            {
                throw new ArgumentException($"CurrencyLayer exchange rate provider requires {response.Source} as source currency");
            }

            var rates = response.GetNormalizedRates();

            return currenciesList
                .Where(currency => rates.ContainsKey(currency.Code) && !currency.Equals(sourceCurrency))
                .Select(currency => new ExchangeRate(
                    sourceCurrency,
                    currency,
                    rates[currency.Code]));
        }
    }
}
