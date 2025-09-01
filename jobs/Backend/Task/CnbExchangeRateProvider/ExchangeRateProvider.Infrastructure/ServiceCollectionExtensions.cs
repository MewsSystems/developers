using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Domain.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure CNB Cache Strategy
            services.AddCnbCacheStrategy(options =>
            {
                // Override defaults from configuration if needed
                var configSection = configuration.GetSection("CnbCacheStrategy");
                if (configSection.Exists())
                {
                    options.PublicationWindowDuration = TimeSpan.FromMinutes(
                        configSection.GetValue<int>("PublicationWindowMinutes", 5));
                    options.WeekdayDuration = TimeSpan.FromHours(
                        configSection.GetValue<int>("WeekdayHours", 1));
                    options.WeekendDuration = TimeSpan.FromHours(
                        configSection.GetValue<int>("WeekendHours", 12));
                }
            });

            var redisEnabled = configuration.GetValue<bool>("Redis:Enabled", false);
            if (redisEnabled)
            {
                var redisConfig = configuration["Redis:Configuration"] ?? "localhost:6379";
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConfig;
                    options.InstanceName = configuration.GetValue<string>("Redis:InstanceName", "ExchangeRates");
                });
            }

            // Configure HttpClient for CnbExchangeRateProvider with Polly for resilience
            services.AddHttpClient("CnbExchangeRateProvider", client =>
            {
                client.BaseAddress = new Uri(configuration["ExchangeRateProvider:CnbExchangeRateUrl"] ?? "https://www.cnb.cz");
                client.Timeout = TimeSpan.FromSeconds(configuration.GetValue<int>("ExchangeRateProvider:TimeoutSeconds", 30));
                client.DefaultRequestHeaders.Add("User-Agent", "ExchangeRateProvider/1.0");
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            // Register the main exchange rate provider
            services.AddScoped<CnbExchangeRateProvider>();

            // Register the provider registration service
            services.AddSingleton<IProviderRegistrationService>(provider =>
                new ProviderRegistrationService(provider, provider.GetRequiredService<IProviderRegistry>()));

            // Register the hosted service for provider registration
            services.AddHostedService<ProviderRegistrationHostedService>();

            // Register the caching decorator as the main IExchangeRateProvider
            services.AddScoped<IExchangeRateProvider>(provider =>
            {
                var cnbProvider = provider.GetRequiredService<CnbExchangeRateProvider>();

                if (redisEnabled)
                {
                    var distributedCache = provider.GetRequiredService<IDistributedCache>();
                    var cacheStrategy = provider.GetRequiredService<CnbCacheStrategy>();
                    var logger = provider.GetRequiredService<ILogger<DistributedCachingExchangeRateProvider>>();

                    return new DistributedCachingExchangeRateProvider(cnbProvider, distributedCache, cacheStrategy, logger);
                }
                else
                {
                    // No caching if Redis is disabled
                    return cnbProvider;
                }
            });


            return services;
        }


        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        var reason = outcome.Result?.StatusCode.ToString() ?? outcome.Exception?.Message ?? "Unknown";
                        Console.WriteLine($"CNB API retry attempt {retryAttempt} after {timespan.TotalSeconds}s due to {reason}");
                    });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, breakDelay) =>
                    {
                        var reason = outcome.Result?.StatusCode.ToString() ?? outcome.Exception?.Message ?? "Unknown";
                        Console.WriteLine($"CNB API circuit breaker opened for {breakDelay.TotalSeconds}s due to {reason}");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("CNB API circuit breaker reset");
                    },
                    onHalfOpen: () =>
                    {
                        Console.WriteLine("CNB API circuit breaker half-open");
                    });
        }
    }
}