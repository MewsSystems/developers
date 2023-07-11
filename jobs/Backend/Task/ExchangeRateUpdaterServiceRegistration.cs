using ExchangeRateUpdater.ApiClients.CzechNationalBank;
using ExchangeRateUpdater.Persistence;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Refit;
using System;

namespace ExchangeRateUpdater;

/// <summary>
/// Provides extension methods for registering Exchange Rate Updater services.
/// </summary>
internal static class ExchangeRateUpdaterServiceRegistration
{
    /// <summary>
    /// Adds Exchange Rate Updater services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing the configuration settings.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddExchangeRateUpdaterServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddScoped<ExchangeRateProvider>();
        services.AddScoped<IExchangeRateProvider, CachedExchangeRateProvider>();

        services.AddRefitClient<IExchangeRateApiClient>()
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            }))
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["CzechNationalBankApi:BaseUrl"]));

        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();

        return services;
    }
}
