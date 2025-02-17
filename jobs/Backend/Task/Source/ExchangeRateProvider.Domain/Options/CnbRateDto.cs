namespace ExchangeRateProvider.Domain.Options;

/// <summary>
///     Configuration options for the CNB API integration.
/// </summary>
public class CnbApiOptions
{
    /// <summary>
    ///     Base URL of the CNB API.
    /// </summary>
    public required string BaseUrl { get; set; }

    /// <summary>
    ///     Number of retry attempts in case of transient HTTP failures.
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    ///     Delay (in milliseconds) between retry attempts.
    /// </summary>
    public int RetryDelayMilliseconds { get; set; }

    /// <summary>
    ///     Lifetime (in minutes) for HTTP handler instances.
    /// </summary>
    public int HandlerLifetimeMinutes { get; set; }

    /// <summary>
    ///     Duration (in hours) for caching exchange rate responses.
    /// </summary>
    public int CacheDurationHours { get; set; }

    /// <summary>
    ///     Hour of the day when the exchange rates should be updated.
    /// </summary>
    public int UpdateHour { get; set; }

    /// <summary>
    ///     Minute of the hour when the exchange rates should be updated.
    /// </summary>
    public int UpdateMinute { get; set; }
}
