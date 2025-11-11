using System;
using System.Net.Http;
using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly.Extensions.Http;
using Polly;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Infrastructure.Observability;

namespace ExchangeRateUpdater.Infrastructure.CNB.Registry
{
    // The infrastrucutre folder could be (in a bigger project) a separated project.
    internal static class InfrastructureServiceCollection
    {
        public static IServiceCollection AddCNBInfrastructure(this IServiceCollection services, IConfiguration config)
            => services
            .AddCNBOptions(config)
            .AddSingleton<IExchangeRateParser, CNBExchangeRateParser>()
            .AddSingleton<CNBExchangeRateFetcher, CNBExchangeRateFetcher>()
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IExchangeRateFetcher>(serviceProvider =>
            {
                return new CachedExchangeRateFetcher(
                    serviceProvider.GetRequiredService<IDistributedCache>(),
                    serviceProvider.GetRequiredService<CNBExchangeRateFetcher>());
            })
            .AddDistributedMemoryCache()
            .AddHttpClient(config)
            .Services;

        private static IHttpClientBuilder AddHttpClient(this IServiceCollection services, IConfiguration config) => 
            services
                .AddHttpClient<CNBExchangeRateFetcher>((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<CNBOptions>>()
                        .Value;

                    client.BaseAddress = new Uri(options.BaseUrl);
                })
                .AddPolicyHandler((serviceProvider, request) =>
                {
                    var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("RetryPolicy");
                    return GetRetryPolicy(logger, serviceProvider.GetRequiredService<Metrics>());
                })
                .AddPolicyHandler((serviceProvider, request) =>
                {
                    var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("CircuitBreakerPolicy");
                    return GetCircuitBreakerPolicy(logger, serviceProvider.GetRequiredService<Metrics>());
                });

        private static IServiceCollection AddCNBOptions(this IServiceCollection services, IConfiguration config) =>
            services.Configure<CNBOptions>(config.GetSection(key: nameof(CNBOptions)));

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger, Metrics metrics)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                retryCount: 3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    logger.LogWarning("Retry {RetryAttempt} for {PolicyKey} at {Time}. Reason: {Result}",
                        retryAttempt,
                        context.PolicyKey,
                        DateTime.UtcNow,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());

                    metrics.RetryCounter.Add(1);
                });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ILogger logger, Metrics metrics)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (request, timespan) =>
                    {
                        logger.LogWarning("Circuit breaker opened at {Time}.",
                            DateTime.UtcNow);

                        metrics.CircuitBreakerOpened.Add(1);
                    },
                    onReset: () =>
                    {
                        logger.LogWarning("Circuit breaker reset at {Time}.",
                            DateTime.UtcNow);

                        metrics.CircuitBreakerReset.Add(1);
                    });
        }
    }
}
