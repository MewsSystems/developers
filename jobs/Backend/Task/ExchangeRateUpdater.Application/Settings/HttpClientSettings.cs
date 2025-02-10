namespace ExchangeRateUpdater.Infrastructure.Configuration;

/// <summary>
/// Configuration settings for HttpClient resiliency policies.
/// </summary>
public class HttpClientSettings
{
    /// <summary>
    /// Gets or sets the global timeout duration for HTTP requests in seconds.
    /// </summary>
    /// <value>Defaults to <c>6</c> seconds.</value>
    public int GlobalTimeoutSeconds { get; set; } = 10;

    /// <summary>
    /// Gets or sets the maximum number of retry attempts for failed requests.
    /// </summary>
    /// <value>Defaults to <c>5</c> attempts.</value>
    public int RetryMaxAttempts { get; set; } = 5;

    /// <summary>
    /// Gets or sets the backoff delay between retry attempts in seconds.
    /// </summary>
    /// <value>Defaults to <c>2</c> seconds.</value>
    public int RetryBackoffSeconds { get; set; } = 2;

    /// <summary>
    /// Gets or sets the duration for sampling failures in the circuit breaker in seconds.
    /// </summary>
    /// <value>Defaults to <c>10</c> seconds.</value>
    public int CircuitBreakerSamplingDurationSeconds { get; set; } = 10;

    /// <summary>
    /// Gets or sets the duration the circuit breaker remains open before resetting in seconds.
    /// </summary>
    /// <value>Defaults to <c>5</c> seconds.</value>
    public int CircuitBreakerBreakDurationSeconds { get; set; } = 5;

    /// <summary>
    /// Gets or sets the failure ratio threshold for the circuit breaker to open.
    /// </summary>
    /// <value>Defaults to <c>0.2</c> (20% failure rate).</value>
    public double CircuitBreakerFailureRatio { get; set; } = 0.2;
}
