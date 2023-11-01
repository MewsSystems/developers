using Mews.ExchangeRateUpdater.Application.Interfaces;
using Mews.ExchangeRateUpdater.Infrastructure.Data.Repositories;
using Mews.ExchangeRateUpdater.Infrastructure.HttpClients;
using Mews.ExchangeRateUpdater.Infrastructure.HttpClients.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Mews.ExchangeRateUpdater.Infrastructure;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add infrastructure dependencies.
    /// </summary>
    /// <param name="services">The collection of DI services to add the infrastructure services to.</param>
    /// <param name="configuration">The application's configuration, used to configure settings and options.</param>
    /// <returns>The original <see cref="IServiceCollection"/> instance, for chaining further registrations if needed.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register repositories
        _ = services.AddMemoryCache();
        _ = services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();

        // Add settings
        _ = services.Configure<ApiUrlsSettings>(configuration.GetSection(ApiUrlsSettings.SectionName));

        // Register HTTP clients
        var apiUrlsSettings = services.BuildServiceProvider().GetRequiredService<IOptions<ApiUrlsSettings>>().Value;
        _ = services
            .AddHttpClient<ICzechNationalBankApiClient, CzechNationalBankApiClient>(client =>
                client.BaseAddress = new Uri(apiUrlsSettings.CzechNationalBankApi))
            .AddRetryPolicy();

        return services;
    }
}