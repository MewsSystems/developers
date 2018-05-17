using ExchangeRateUpdater.ExchangeRateStrategies;
using ExchangeRateUpdater.Factory.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Factory
{
    public class ConfigurableProviderFactory : IProviderFactory
    {
        private readonly IProviderFactoryConfig _config;

        public ConfigurableProviderFactory(IProviderFactoryConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Returns exchange rate provider for specified source currency or null provider if not found.
        /// </summary>
        /// <param name="sourceCurrency">Source currency</param>
        /// <returns>Exchange rate provider</returns>
        public IExchangeRateProviderSourceCurrencyStrategy GetSourceCurrencyProvider(Currency sourceCurrency)
            => _config.SourceCurrencyProviderFactories.ContainsKey(sourceCurrency)
                ? _config.SourceCurrencyProviderFactories[sourceCurrency]()
                : new NullExchangeRateProviderStrategy();

        /// <summary>
        /// Returns exchange rate provider for specified target currency or null provider if not found.
        /// </summary>
        /// <param name="targetCurrency">Target currency</param>
        /// <returns>Exchange rate provider</returns>
        public IExchangeRateProviderTargetCurrencyStrategy GetTargetCurrencyProvider(Currency targetCurrency)
            => _config.TargetCurrencyProviderFactories.ContainsKey(targetCurrency)
                ? _config.TargetCurrencyProviderFactories[targetCurrency]()
                : new NullExchangeRateProviderStrategy();

        /// <summary>
        /// Returns pairs of exchange rate providers and their base currencies for specified list of currencies.
        /// Returns both source and target currency providers.
        /// </summary>
        /// <param name="currencies">List of currencies</param>
        /// <returns>List of exchange rate providers and their base currencies</returns>
        public IEnumerable<ProviderInfo> GetProviders(IEnumerable<Currency> currencies)
        {
            var currenciesList = currencies as IList<Currency> ?? currencies.ToList();

            return GetSourceCurrencyProviders(currenciesList)
                .Concat(GetTargetCurrencyProviders(currenciesList));
        }

        private IEnumerable<ProviderInfo> GetSourceCurrencyProviders(IEnumerable<Currency> currencies)
            => currencies
                .Where(currency => _config.SourceCurrencyProviderFactories.ContainsKey(currency))
                .Select(currency => new ProviderInfo(
                    currency,
                    _config.SourceCurrencyProviderFactories[currency]()));
        
        private IEnumerable<ProviderInfo> GetTargetCurrencyProviders(IEnumerable<Currency> currencies)
            => currencies
                .Where(currency => _config.TargetCurrencyProviderFactories.ContainsKey(currency))
                .Select(currency => new ProviderInfo(
                    currency,
                    _config.TargetCurrencyProviderFactories[currency]()));
    }
}
