using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater.Providers
{
    internal class ExchangeRateProviderFactory : IExchangeRateProviderFactory
    {
        private readonly IReadOnlyDictionary<string, IRateProvider> _rateProviders;

        public ExchangeRateProviderFactory(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            var exchangeRateProviderType = typeof(IRateProvider);

            IEnumerable<Type> allProviders = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => exchangeRateProviderType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            IEnumerable<object> rateProviders = allProviders.Select(p =>
            {
                return Activator.CreateInstance(p, httpClient, loggerFactory.CreateLogger(p.Name));
            });

            _rateProviders = rateProviders.Cast<IRateProvider>().ToDictionary(p => p.BaseCurrencyCode, p=>p);
        }

        public IReadOnlyDictionary<string, IRateProvider> GetRateProviders()
        {
            return _rateProviders;
        }

    }
}
