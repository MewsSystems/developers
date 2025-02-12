using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Infrastructure.Caching;
using ExchangeRateUpdater.Infrastructure.Clients;
using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Infrastructure.Interfaces;
using ExchangeRateUpdater.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using System;

namespace ExchangeRateUpdater.Infrastructure.DependencyInjection;

/// <summary>
/// Provides extension methods for registering infrastructure dependencies in the DI container.
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// Registers infrastructure services, including HTTP clients, caching and external data providers.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="httpClientSettings">Configuration settings for HTTP client resilience policies.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> with registered services.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, HttpClientSettings httpClientSettings)
    {
        // Register CNB API client with resilience policies
        services
            .AddHttpClient<ICnbApiClient, CnbApiClient>()
            .AddResilienceHandler("DefaultResilience", (pipeline, context) =>
            {
                pipeline
                    .AddTimeout(TimeSpan.FromSeconds(httpClientSettings.GlobalTimeoutSeconds))
                    .AddRetry(new HttpRetryStrategyOptions
                    {
                        BackoffType = DelayBackoffType.Exponential,
                        Delay = TimeSpan.FromSeconds(httpClientSettings.RetryBackoffSeconds),
                        MaxRetryAttempts = httpClientSettings.RetryMaxAttempts,
                    })
                    .AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                    {
                        SamplingDuration = TimeSpan.FromSeconds(httpClientSettings.CircuitBreakerSamplingDurationSeconds),
                        FailureRatio = httpClientSettings.CircuitBreakerFailureRatio,
                        BreakDuration = TimeSpan.FromSeconds(httpClientSettings.CircuitBreakerBreakDurationSeconds),
                    })
                    .Build();
            });

        // Register core services
        services.AddSingleton<ICacheService, CacheService>();
        services.AddScoped<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
        services.AddMemoryCache();

        return services;
    }
}
