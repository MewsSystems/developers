using System;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Countries.CZE;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Factories;

public class ExchangeRateProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ExchangeRateProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IExchangeRateProvider CreateProvider(CountryIsoAlpha3 country)
    {
        return country switch
        {
            CountryIsoAlpha3.CZE => _serviceProvider.GetRequiredService<CzeExchangeRateProvider>(),
            _ => _serviceProvider.GetRequiredService<ExchangeRateProvider>()
        };
    }
}
