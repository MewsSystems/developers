using System;
using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var exchangeOptions = new ExchangeOptions();
        configuration.GetSection("ExchangeOptions").Bind(exchangeOptions);
        serviceCollection.AddSingleton(exchangeOptions);

        return serviceCollection;
    }

    public static IServiceCollection AddCnbHttpClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient<ICnbHttpClient, CnbHttpClient>((sp, httpClient) =>
        {
            var baseUrl = sp.GetRequiredService<ExchangeOptions>().CnbRatesUrl;
            httpClient.BaseAddress = new Uri(baseUrl);
        });
        
        return serviceCollection;
    }

    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        
        return serviceCollection;
    }
}