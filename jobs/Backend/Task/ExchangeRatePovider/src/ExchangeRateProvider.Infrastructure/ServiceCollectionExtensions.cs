using ExchangeRateProvider.Application.Interfaces;
using ExchangeRateProvider.Infrastructure.ExchangeRateProviders;
using ExchangeRateProvider.Infrastructure.ExchangeRateProviders.Cnb;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider.Infrastructure;

/// <summary>
/// Provides extension methods for configuring infrastructure layer services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers infrastructure layer services including exchange rate providers and HTTP clients with the dependency injection container.
    /// Configures the CNB (Czech National Bank) exchange rate provider with its associated HTTP client and options.
    /// </summary>
    /// <param name="services">The service collection to add infrastructure services to.</param>
    /// <param name="configuration">The configuration containing settings for infrastructure services, including CNB API options.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var cnbApiOptionsSection = configuration.GetSection(CnbApiOptions.SectionName);
        var cnbApiOptions = cnbApiOptionsSection.Get<CnbApiOptions>()!;
        
        services.Configure<CnbApiOptions>(cnbApiOptionsSection);

        services.AddHttpClient<IExchangeRateProvider, CnbExchangeRateProvider>(
            cnbApiOptions.HttpClientName, client =>
            {
                client.BaseAddress = new Uri(cnbApiOptions.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddResilience("cnb-resilience");

        services.AddMemoryCache();

        services.AddSingleton(TimeProvider.System);

        services.AddSingleton<ICacheDateTimeProvider, CnbCacheDateTimeProvider>();

        services.AddScoped<CnbExchangeRateProvider>();

        services.AddScoped<IExchangeRateProvider>(serviceProvider =>
        {
            var inner = serviceProvider.GetRequiredService<CnbExchangeRateProvider>();
            var cache = serviceProvider.GetRequiredService<IMemoryCache>();
            var dateTimeProvider = serviceProvider.GetRequiredService<ICacheDateTimeProvider>();
            var logger = serviceProvider.GetRequiredService<ILogger<CnbCachedExchangeRateProviderDecorator>>();

            return new CnbCachedExchangeRateProviderDecorator(inner, cache, dateTimeProvider, logger);
        });

        return services;
    }
}
