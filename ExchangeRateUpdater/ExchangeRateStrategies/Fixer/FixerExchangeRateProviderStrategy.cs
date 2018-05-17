using ExchangeRateUpdater.ExchangeRateStrategies.Fixer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateStrategies.Fixer
{
    public class FixerExchangeRateProviderStrategy : IExchangeRateProviderSourceCurrencyStrategy
    {
        public const string ApiKey = "4c8456c3f0cc9151786daa7387190e7c";

        private readonly IFixerRatesFetcher _fetcher;

        public FixerExchangeRateProviderStrategy(IFixerRatesFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(Currency sourceCurrency, IEnumerable<Currency> currencies)
        {
            var currenciesList = currencies as IList<Currency> ?? currencies.ToList();

            var response = await _fetcher.FetchRatesAsync(ApiKey, currenciesList.Select(currency => currency.Code));

            if (!sourceCurrency.Equals(new Currency(response.Base)))
            {
                throw new ArgumentException($"Fixer exchange rate provider requires {response.Base} as source currency");
            }

            return currenciesList
                .Where(currency => response.Rates.ContainsKey(currency.Code) && !currency.Equals(sourceCurrency))
                .Select(currency => new ExchangeRate(
                    sourceCurrency,
                    currency,
                    response.Rates[currency.Code]));
        }
    }
}
