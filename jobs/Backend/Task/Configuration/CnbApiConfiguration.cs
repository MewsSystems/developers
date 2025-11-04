namespace ExchangeRateUpdater.Configuration;

/// <summary>
/// Configuration settings for the Czech National Bank API client.
/// </summary>
public class CnbApiConfiguration
{
    /// <summary>
    /// The URL endpoint for retrieving daily exchange rates from CNB.
    /// </summary>
    public string CnbDailyRatesUrl { get; set; }

    /// <summary>
    /// The timeout duration in seconds for HTTP requests to the CNB API.
    /// Default value is 30 seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// The number of retry attempts for failed HTTP requests.
    /// Default value is 3 attempts.
    /// </summary>
    public int RetryCount { get; set; } = 3;
}
