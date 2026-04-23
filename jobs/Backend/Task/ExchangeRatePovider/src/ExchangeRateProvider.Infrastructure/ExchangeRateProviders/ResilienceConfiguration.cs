namespace ExchangeRateProvider.Infrastructure.ExchangeRateProviders;

/// <summary>
/// Contains configuration constants for HTTP client resilience patterns.
/// These values define timeout, retry, and circuit breaker behavior for external API calls.
/// </summary>
internal static class ResilienceConfiguration
{
    /// <summary>
    /// Maximum time to wait for a single HTTP request before timing out.
    /// </summary>
    public static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Maximum number of retry attempts for failed HTTP requests.
    /// </summary>
    public const int MaxRetryAttempts = 3;

    /// <summary>
    /// Initial delay before the first retry attempt.
    /// </summary>
    public static readonly TimeSpan InitialRetryDelay = TimeSpan.FromMilliseconds(200);

    /// <summary>
    /// Whether to use jitter in retry delays to avoid thundering herd problems.
    /// </summary>
    public const bool UseJitter = true;

    /// <summary>
    /// Time window for sampling circuit breaker failure rate.
    /// </summary>
    public static readonly TimeSpan CircuitBreakerSamplingDuration = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Failure ratio threshold (0.0 to 1.0) that triggers circuit breaker to open.
    /// 0.5 means 50% of requests must fail within the sampling duration.
    /// </summary>
    public const double CircuitBreakerFailureRatio = 0.5;

    /// <summary>
    /// Minimum number of requests required before circuit breaker can open.
    /// This prevents opening on a small number of requests.
    /// </summary>
    public const int CircuitBreakerMinimumThroughput = 10;

    /// <summary>
    /// How long the circuit breaker stays open before attempting to close.
    /// </summary>
    public static readonly TimeSpan CircuitBreakerBreakDuration = TimeSpan.FromSeconds(30);
}