using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Retry;

namespace ExchangeRateProvider.Infrastructure.ExchangeRateProviders;

/// <summary>
/// Provides extension methods for adding resilience patterns to HTTP clients.
/// </summary>
internal static class HttpClientBuilderExtensions
{
    /// <summary>
    /// Adds a resilience pipeline to the HTTP client with timeout, retry, and circuit breaker policies.
    /// The pipeline includes timeout, exponential backoff retry strategy, and circuit breaker policies
    /// with values defined in <see cref="ResilienceConfiguration"/>.
    /// </summary>
    /// <param name="builder">The HTTP client builder to add resilience to.</param>
    /// <param name="name">The name of the resilience handler for identification purposes.</param>
    /// <returns>The HTTP resilience pipeline builder for further configuration.</returns>
    internal static IHttpResiliencePipelineBuilder AddResilience(this IHttpClientBuilder builder, string name)
    {
        return builder.AddResilienceHandler(name, pipeline =>
        {
            pipeline.AddTimeout(ResilienceConfiguration.Timeout);

            pipeline.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = ResilienceConfiguration.MaxRetryAttempts,
                Delay = ResilienceConfiguration.InitialRetryDelay,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = ResilienceConfiguration.UseJitter,
                ShouldHandle = CreateRetryPredicateBuilder()
            });

            pipeline.AddCircuitBreaker(new Polly.CircuitBreaker.CircuitBreakerStrategyOptions<HttpResponseMessage>
            {
                SamplingDuration = ResilienceConfiguration.CircuitBreakerSamplingDuration,
                FailureRatio = ResilienceConfiguration.CircuitBreakerFailureRatio,
                MinimumThroughput = ResilienceConfiguration.CircuitBreakerMinimumThroughput,
                BreakDuration = ResilienceConfiguration.CircuitBreakerBreakDuration,
                ShouldHandle = CreateCircuitBreakerPredicateBuilder()
            });
        });
    }

    /// <summary>
    /// Creates the predicate builder for determining which conditions should trigger retry attempts.
    /// </summary>
    /// <returns>A predicate builder configured for retry scenarios.</returns>
    private static PredicateBuilder<HttpResponseMessage> CreateRetryPredicateBuilder()
    {
        return new PredicateBuilder<HttpResponseMessage>()
            .Handle<HttpRequestException>()
            .Handle<TaskCanceledException>()
            .HandleResult(r =>
                r.StatusCode == HttpStatusCode.RequestTimeout ||
                r.StatusCode == HttpStatusCode.TooManyRequests ||
                (int)r.StatusCode >= 500);
    }

    /// <summary>
    /// Creates the predicate builder for determining which conditions should be counted as failures by the circuit breaker.
    /// </summary>
    /// <returns>A predicate builder configured for circuit breaker scenarios.</returns>
    private static PredicateBuilder<HttpResponseMessage> CreateCircuitBreakerPredicateBuilder()
    {
        return new PredicateBuilder<HttpResponseMessage>()
            .Handle<HttpRequestException>()
            .Handle<TaskCanceledException>()
            .HandleResult(r =>
                (int)r.StatusCode >= 500 ||
                r.StatusCode == HttpStatusCode.TooManyRequests);
    }
}
