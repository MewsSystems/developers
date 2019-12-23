using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderFactory : IExchangeRateProviderFactory
    {
        private readonly IEnumerable<IExchangeRateProvider> availableProviders;

        public ExchangeRateProviderFactory(IEnumerable<IExchangeRateProvider> availableProviders)
        {
            this.availableProviders = availableProviders;
        }

        public IExchangeRateProvider GetExchangeRateProvider(ProviderName providerName)
        {
            var provider = availableProviders.FirstOrDefault(x => x.ProviderName == providerName);
            if (provider == null)
            {
                throw new InvalidOperationException($"ExchangeRateProvider '{providerName}' was not found.");
            }

            return provider;
        }
    }
}
