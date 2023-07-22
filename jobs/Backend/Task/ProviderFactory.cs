using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.ApiClients;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.ApiClients.Implementations;

namespace ExchangeRateUpdater;

public static class ProviderFactory
{
    public static IServiceCollection AddExchangeRatesProvider(this IServiceCollection services, IConfiguration configuration)
    {
        var configuredProvider = configuration["ExchangeRateProvider"];
        
        if (string.IsNullOrEmpty(configuredProvider))
            throw new ArgumentException("No provider has been configured");

        return configuredProvider switch
        {
            ("CzechNationalBank") => services.AddCzechNationalBankExchangeRateProvider(configuration),
            _ => throw new ArgumentException(
                $"An invalid provider has been configured provider {configuredProvider} is not available")
        };
    }

    private static IServiceCollection AddCzechNationalBankExchangeRateProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IApiClient, CnbApi>();
        services.AddHttpClient("CzechNationalBankApi",
                cfg => { cfg.BaseAddress = new Uri(configuration["Providers:CzechNationalBank:Api:BaseUrl"]); })
            .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3)));
        services.AddScoped<IExchangeRateProviderService, CzechNationalBankExchangeRateProviderService>();
        services.AddMemoryCache();
        return services;
    }
}