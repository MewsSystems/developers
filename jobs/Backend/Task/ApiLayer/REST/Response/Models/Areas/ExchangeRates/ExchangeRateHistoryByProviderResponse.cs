using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for exchange rate history grouped by provider.
/// Avoids duplicating provider information for each historical rate entry.
/// </summary>
public class ExchangeRateHistoryByProviderResponse
{
    /// <summary>
    /// Provider information.
    /// </summary>
    public ProviderInfo Provider { get; set; } = new();

    /// <summary>
    /// Source currency code (base currency of the provider).
    /// </summary>
    public string SourceCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Collection of historical rate entries from this provider.
    /// </summary>
    public List<HistoricalRateItem> Rates { get; set; } = new();

    /// <summary>
    /// Total number of historical entries.
    /// </summary>
    public int TotalEntries => Rates.Count;
}

/// <summary>
/// Individual historical rate item within a provider's history collection.
/// </summary>
public class HistoricalRateItem
{
    /// <summary>
    /// Currency pair for this exchange rate.
    /// </summary>
    public CurrencyPair CurrencyPair { get; set; } = new();

    /// <summary>
    /// Rate information including multiplier and effective rate.
    /// </summary>
    public RateInfo RateInfo { get; set; } = new();

    /// <summary>
    /// Date for which this rate is valid (YYYY-MM-DD).
    /// </summary>
    public string ValidDate { get; set; } = string.Empty;

    /// <summary>
    /// Change from the previous day's rate (absolute value).
    /// </summary>
    public decimal? ChangeFromPrevious { get; set; }

    /// <summary>
    /// Percentage change from the previous day.
    /// </summary>
    public decimal? ChangePercentage { get; set; }

    /// <summary>
    /// Indicates if the rate increased compared to previous day.
    /// </summary>
    public bool? IsIncreasing => ChangeFromPrevious > 0;

    /// <summary>
    /// Indicates if the rate decreased compared to previous day.
    /// </summary>
    public bool? IsDecreasing => ChangeFromPrevious < 0;
}
