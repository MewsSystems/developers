using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for current exchange rates grouped by base currency.
/// Shows all target currencies available from a single base currency.
/// </summary>
public class CurrentExchangeRatesByBaseCurrencyResponse
{
    /// <summary>
    /// Base currency code for this group.
    /// </summary>
    public string BaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Collection of current exchange rates to various target currencies.
    /// </summary>
    public List<CurrentRateByBaseCurrencyItem> Rates { get; set; } = new();

    /// <summary>
    /// Total number of target currencies available.
    /// </summary>
    public int TotalTargetCurrencies => Rates.Count;
}

/// <summary>
/// Individual current rate item for a target currency.
/// </summary>
public class CurrentRateByBaseCurrencyItem
{
    /// <summary>
    /// Provider information for this rate.
    /// </summary>
    public ProviderInfo Provider { get; set; } = new();

    /// <summary>
    /// Target currency code.
    /// </summary>
    public string TargetCurrency { get; set; } = string.Empty;

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
