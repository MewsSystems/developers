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

        var asyncPolicy = new AsyncPolicyFactory(logger).CreateAsyncRetryPolicy();

        var czechNationalBankApiSettings = new ExchangeRatesSearcherService.CzechNationalBankApiSettings(
            settings.CzechNationalBankApiSettings.ApiBaseAddress,
            settings.CzechNationalBankApiSettings.Delimiter,
            settings.CzechNationalBankApiSettings.DecimalSeparator);
        
        serviceCollection
            .AddSingleton(logger)
            .AddSingleton(settings)
            .AddSingleton(czechNationalBankApiSettings)
            .AddHttpClient<IExchangeRatesSearcher, ExchangeRatesSearcherService.ExchangeRatesSearcherService>(httpClient => httpClient.Timeout = TimeSpan.FromSeconds(10))
            .AddPolicyHandler(asyncPolicy);
            
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public IExchangeRatesSearcher GetExchangeRatesSearcherService()
    {
        return _serviceProvider.GetService<IExchangeRatesSearcher>()!;
    }
}