namespace ExchangeRateUpdater.Domain.Options;

/// <summary>
/// Application settings
/// </summary>
public class ApplicationOptions
{
    /// <summary>
    /// The base exchange rate currency
    /// </summary>
    public string ExchangeRateCurrency { get; set; }

    /// <summary>
    /// Use for in memory cache
    /// </summary>
    public bool EnableInMemoryCache { get; set; }
}