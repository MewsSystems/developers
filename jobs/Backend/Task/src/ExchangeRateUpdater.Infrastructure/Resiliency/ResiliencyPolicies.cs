using Microsoft.Extensions.Http.Resilience;
using Polly;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdater.Infrastructure.Resiliency
{
    [ExcludeFromCodeCoverage]
    public static class ResiliencyPolicies
    {
        /// <summary>
        /// Pipeline with Retry and Circuit Breaker policies.
        /// 
        /// 5 retries with exponential delay between retry attempts.
        /// 
        /// The circuit will break if more than 20% of actions result in handled exceptions,
        /// within any 10-second sampling duration, and at least 10 actions are processed.
        /// 
        /// Global timeout of 5 seconds for all request, including retries, to perform our request
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="retriesBeforeBreaking"></param>
        public static void GetRetryAndCircuitBreakerPolicy(ResiliencePipelineBuilder<HttpResponseMessage> builder)
            => builder
            // https://www.pollydocs.org/strategies/timeout.html
            .AddTimeout(TimeSpan.FromSeconds(5)) // Global timeout
            .AddRetry(new HttpRetryStrategyOptions
            {
                // https://www.pollydocs.org/strategies/retry.html
                MaxRetryAttempts = 5,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromSeconds(2),
            })
            .AddTimeout (TimeSpan.FromSeconds(1)) // Per request timeout
            .AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                // https://www.pollydocs.org/strategies/circuit-breaker.html
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromSeconds(5),
                MinimumThroughput = 10,
                FailureRatio = 0.2,
            })
            .Build();
    }
}
