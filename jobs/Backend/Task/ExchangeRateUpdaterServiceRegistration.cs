using ExchangeRateUpdater.ApiClients.CzechNationalBank;
using ExchangeRateUpdater.Persistence;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddRefitClient<IExchangeRateApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["CzechNationalBankApi:BaseUrl"]));

        services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();

        return services;
    }
}
