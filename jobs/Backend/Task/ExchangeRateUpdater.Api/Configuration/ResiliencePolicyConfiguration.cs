using ExchangeRate.Api.Clients;
using Polly;
using Polly.RateLimiting;
using Polly.Retry;

namespace ExchangeRate.Api.Configuration;

public static class ResiliencePolicyConfiguration
{
    public static IServiceCollection ConfigureResiliencePolicy(this IServiceCollection services)
    {
        return services
            .ConfigureHttpClientResiliencePolicy()
            .ConfigureDefaultResiliencePolicy();
    }

    private static IServiceCollection ConfigureHttpClientResiliencePolicy(this IServiceCollection services)
    {
        return services
            .AddHttpClient<IExchangeRateClient>()
            .AddStandardResilienceHandler()
            .Services;
    }

    private static IServiceCollection ConfigureDefaultResiliencePolicy(this IServiceCollection services)
    {
        return services
            .AddResiliencePipeline("default", resiliencePipelineBuilder =>
            {
                resiliencePipelineBuilder.AddRetry(new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1)
                });

                resiliencePipelineBuilder.AddRateLimiter(new RateLimiterStrategyOptions());
            });
    }
}