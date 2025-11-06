namespace ExchangeRateUpdater.Configuration;

/// <summary>
/// Configuration settings for the Czech National Bank exchange rate provider.
/// </summary>
public class CnbExchangeRateConfiguration
{
    public const string SectionName = "CnbExchangeRate";

    /// <summary>
    /// The base URL for CNB daily exchange rates.
    /// </summary>
    public string ApiUrl { get; set; } = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

    /// <summary>
    /// Timeout for HTTP requests in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Number of retry attempts for failed requests.
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Delay between retry attempts in milliseconds.
    /// </summary>
    public int RetryDelayMilliseconds { get; set; } = 1000;

    /// <summary>
    /// Enable caching of exchange rates.
    /// </summary>
    public bool EnableCache { get; set; } = true;

    /// <summary>
    /// Cache duration in minutes. Default is 60 minutes (1 hour).
    /// CNB updates rates once per day, so caching for 1 hour is reasonable.
    /// </summary>
    public int CacheDurationMinutes { get; set; } = 60;
}
