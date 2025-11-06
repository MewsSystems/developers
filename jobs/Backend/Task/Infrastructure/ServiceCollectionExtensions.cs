using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// Extension methods for configuring services in the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Exchange Rate Provider and its dependencies to the service collection.
    /// </summary>
    public static IServiceCollection AddExchangeRateProvider(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind configuration
        services.Configure<CnbExchangeRateConfiguration>(
            configuration.GetSection(CnbExchangeRateConfiguration.SectionName));

        // Get configuration for HttpClient setup
        var config = configuration
            .GetSection(CnbExchangeRateConfiguration.SectionName)
            .Get<CnbExchangeRateConfiguration>() ?? new CnbExchangeRateConfiguration();

        // Register caching
        services.AddMemoryCache();
        services.AddSingleton<IExchangeRateCache, ExchangeRateCache>();

        // Register services
        services.AddSingleton<ICnbDataParser, CnbDataParser>();
        services.AddScoped<ExchangeRateProvider>();

        // Register HttpClient with resilience (retry, timeout, circuit breaker)
        services.AddHttpClient<ICnbApiClient, CnbApiClient>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddStandardResilienceHandler(options =>
            {
                // Configure retry
                options.Retry.MaxRetryAttempts = config.RetryCount;
                options.Retry.Delay = TimeSpan.FromMilliseconds(config.RetryDelayMilliseconds);
                options.Retry.BackoffType = Polly.DelayBackoffType.Exponential;

                // Configure timeout
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

                // Configure circuit breaker (sampling duration must be at least 2x timeout)
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(config.TimeoutSeconds * 2 + 10);
            });

        return services;
    }
}
