using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Factories
{
    public class ExchangeRateProviderFactory : IExchangeRateProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ExchangeRateProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IExchangeRateProvider Create(ProviderType type)
        {
            return type switch
            {
                ProviderType.api => _serviceProvider.GetRequiredService<ApiExchangeRateProvider>(),
                ProviderType.text => _serviceProvider.GetRequiredService<TextExchangeRateProvider>(),
                ProviderType.fallback => _serviceProvider.GetRequiredService<ExchangeRateProvider>(),
                _ => throw new ArgumentException($"Invalid type: {type}"),
            };
        }
    }

}
