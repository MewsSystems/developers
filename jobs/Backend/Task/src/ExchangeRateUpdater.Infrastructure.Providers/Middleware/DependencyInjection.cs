using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Refit;

namespace ExchangeRateUpdater.Infrastructure.Providers.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddThirdPartyProviders(this IServiceCollection services, IConfiguration? configuration = null)
    {
        services.AddTransient<RefitLoggingHandler>(); 
        
        if (configuration != null)
        {
            services.AddSingleton(configuration);
            
            services.Configure<ExchangeRateProvidersConfig>(
                configuration.GetSection("ExchangeRateProviders"));
            
            services.Configure<CzechNationalBankExchangeRateConfig>(
                configuration.GetSection("ExchangeRateProviders:CzechNationalBank"));
        }
        
        services.AddRefitClient<ICzechNationalBankApiClient>()
            .ConfigureHttpClient(c =>
            {
                var baseUrl = configuration?["ExchangeRateProviders:CzechNationalBank:BaseUrl"] ?? "https://api.cnb.cz/cnbapi/";
                var timeoutSeconds = configuration?.GetValue<int>("ExchangeRateProviders:CzechNationalBank:TimeoutSeconds") ?? 30;

                c.BaseAddress = new Uri(baseUrl);
                c.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            })
            .AddHttpMessageHandler<RefitLoggingHandler>()
            .AddResilienceHandler("standard-resilience-policy", builder =>
            {
                builder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    ShouldHandle = args => ValueTask.FromResult(
                        args.Outcome.Result?.StatusCode is HttpStatusCode.TooManyRequests or >= HttpStatusCode.InternalServerError
                        || args.Outcome.Exception is not null)
                });

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