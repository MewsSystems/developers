using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Refit;

namespace ExchangeRateUpdater.Infrastructure.Providers.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddThirdPartyProviders(this IServiceCollection services, IConfiguration? configuration = null)
    {
        // Todo Andrei: Review different injection scopes
        // Todo Andrei: Clean up, add exponential backoff
        services.AddTransient<RefitLoggingHandler>();
        
        // Register Refit API clients first
        services.AddRefitClient<ICzechNationalBankApiClient>()
            .ConfigureHttpClient(c =>
            {
                // Use configuration if available, otherwise use defaults
                var baseUrl = configuration?["CzechNationalBank:BaseUrl"] ?? "https://api.cnb.cz/cnbapi/";
                var timeout = configuration?.GetValue<int>("CzechNationalBank:TimeoutSeconds") ?? 30;

                c.BaseAddress = new Uri(baseUrl);
                c.Timeout = TimeSpan.FromSeconds(timeout);
            })
            .AddHttpMessageHandler<RefitLoggingHandler>()
            .AddResilienceHandler("standard-resilience-policy", builder =>
            {
                // Add retry strategy with exponential backoff
                builder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    ShouldHandle = args => ValueTask.FromResult(
                        args.Outcome.Result?.StatusCode is HttpStatusCode.TooManyRequests or >= HttpStatusCode.InternalServerError
                        || args.Outcome.Exception is not null)
                });

                // Add circuit breaker
                builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    FailureRatio = 0.5,
                    MinimumThroughput = 8,
                    BreakDuration = TimeSpan.FromSeconds(15),
                    ShouldHandle = args => ValueTask.FromResult(
                        args.Outcome.Result?.StatusCode is HttpStatusCode.TooManyRequests or >= HttpStatusCode.InternalServerError
                        || args.Outcome.Exception is not null)
                });
            });
        
        // Register exchange rate providers here
        services.AddSingleton<IExchangeRateProvider, CnbExchangeRateProvider>();
         
        return services;
    }
} 