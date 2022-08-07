using System;
using Domain.Ports;
using ExchangeRateUpdater.Host.Console.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ExchangeRateUpdater.Host.Console;

internal class ServicesProviderConfiguration
{
    private ServiceProvider _serviceProvider;

    public void SetupServices(ISettings settings, ILogger logger)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddSingleton(logger)
            .AddSingleton(settings)
            .AddHttpClient<IExchangeRatesSearcher, ExchangeRatesSearcherService.ExchangeRatesSearcherService>(httpClient => httpClient.Timeout = TimeSpan.FromSeconds(10));
            
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public IExchangeRatesSearcher GetExchangeRatesSearcherService()
    {
        return _serviceProvider.GetService<IExchangeRatesSearcher>()!;
    }
}