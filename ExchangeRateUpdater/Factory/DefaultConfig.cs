using ExchangeRateUpdater.ExchangeRateStrategies;
using ExchangeRateUpdater.Factory.Abstract;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Factory
{
    public class DefaultConfig : IProviderFactoryConfig
    {
        public Dictionary<Currency, Func<IExchangeRateProviderSourceCurrencyStrategy>> SourceCurrencyProviderFactories { get; }
        public Dictionary<Currency, Func<IExchangeRateProviderTargetCurrencyStrategy>> TargetCurrencyProviderFactories { get; }

        public DefaultConfig()
        {
            SourceCurrencyProviderFactories = new Dictionary<Currency, Func<IExchangeRateProviderSourceCurrencyStrategy>>();
            TargetCurrencyProviderFactories = new Dictionary<Currency, Func<IExchangeRateProviderTargetCurrencyStrategy>>();
        }

        public DefaultConfig AddSourceCurrencyProvider(Currency sourceCurrency, Func<IExchangeRateProviderSourceCurrencyStrategy> factory)
        {
            SourceCurrencyProviderFactories[sourceCurrency] = factory;
            return this;
        }

        public DefaultConfig AddTargetCurrencyProvider(Currency targetCurrency, Func<IExchangeRateProviderTargetCurrencyStrategy> factory)
        {
            TargetCurrencyProviderFactories[targetCurrency] = factory;
            return this;
        }
    }
}
