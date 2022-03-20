using System.Collections.Generic;

namespace ExchangeRateUpdater.Providers
{
    internal interface IExchangeRateProviderFactory
    {
        public IReadOnlyDictionary<string, IRateProvider> GetRateProviders();
    }
}