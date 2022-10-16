namespace ExchangeRateUpdater.Clients.Cnb.Options;

/// <summary>
/// Retry options.
/// </summary>
public class RetryOptions
{
    /// <summary>
    /// Gets or sets the number of retries.
    /// </summary>
    public int Count { get; set; } = 5;

    /// <summary>
    /// Gets or sets the delay for jitter
    /// </summary>
    public int DelayInSeconds { get; set; } = 1;
}