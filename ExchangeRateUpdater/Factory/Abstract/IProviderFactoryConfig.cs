using ExchangeRateUpdater.ExchangeRateStrategies;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Factory.Abstract
{
    public interface IProviderFactoryConfig
    {
        Dictionary<Currency, Func<IExchangeRateProviderSourceCurrencyStrategy>> SourceCurrencyProviderFactories { get; }

        Dictionary<Currency, Func<IExchangeRateProviderTargetCurrencyStrategy>> TargetCurrencyProviderFactories { get; }
    }
}
