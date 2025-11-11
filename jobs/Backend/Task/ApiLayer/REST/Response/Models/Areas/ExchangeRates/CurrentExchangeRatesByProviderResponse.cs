using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for current exchange rates grouped by provider.
/// Avoids duplicating provider information when multiple rates are returned.
/// </summary>
public class CurrentExchangeRatesByProviderResponse
{
    /// <summary>
    /// Provider information.
    /// </summary>
    public ProviderInfo Provider { get; set; } = new();

    /// <summary>
    /// Collection of current exchange rates from this provider.
    /// </summary>
    public List<CurrentRateItem> Rates { get; set; } = new();

    /// <summary>
    /// Total number of current rates from this provider.
    /// </summary>
    public int TotalRates => Rates.Count;
}

/// <summary>
/// Individual current rate item within a provider's rate collection.
/// </summary>
public class CurrentRateItem
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
    /// Date for which this rate is valid (in YYYY-MM-DD format).
    /// </summary>
    public string ValidDate { get; set; } = string.Empty;

    /// <summary>
    /// When this rate was last updated (ISO 8601 format).
    /// </summary>
    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// Number of days since this rate was updated.
    /// </summary>
    public int DaysOld { get; set; }

    /// <summary>
    /// Indicates if this rate is fresh (less than 2 days old).
    /// </summary>
    public bool IsFresh => DaysOld < 2;
}
