using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for latest exchange rates with nested grouping.
/// Groups by Provider → Base Currency → Target Currencies.
/// Eliminates all duplicate provider and base currency information.
/// </summary>
public class LatestExchangeRatesGroupedResponse
{
    /// <summary>
    /// Provider information.
    /// </summary>
    public ProviderInfo Provider { get; set; } = new();

    /// <summary>
    /// Collection of base currencies offered by this provider.
    /// </summary>
    public List<BaseCurrencyGroup> BaseCurrencies { get; set; } = new();

    /// <summary>
    /// Total number of base currencies.
    /// </summary>
    public int TotalBaseCurrencies => BaseCurrencies.Count;

    /// <summary>
    /// Total number of rates across all base currencies.
    /// </summary>
    public int TotalRates => BaseCurrencies.Sum(bc => bc.TotalTargetCurrencies);
}

/// <summary>
/// Group of exchange rates for a specific base currency within a provider.
/// </summary>
public class BaseCurrencyGroup
{
    /// <summary>
    /// Base currency code for this group.
    /// </summary>
    public string BaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Collection of rates to various target currencies.
    /// </summary>
    public List<TargetCurrencyRate> Rates { get; set; } = new();

    /// <summary>
    /// Total number of target currencies available from this base.
    /// </summary>
    public int TotalTargetCurrencies => Rates.Count;
}

/// <summary>
/// Individual exchange rate to a target currency.
/// </summary>
public class TargetCurrencyRate
{
    /// <summary>
    /// Exchange rate unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Target currency code.
    /// </summary>
    public string TargetCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Rate information including multiplier and effective rate.
    /// </summary>
    public RateInfo RateInfo { get; set; } = new();

    /// <summary>
    /// Valid date for this rate (YYYY-MM-DD format).
    /// </summary>
    public string ValidDate { get; set; } = string.Empty;

    /// <summary>
    /// When this rate was created in the system.
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// When this rate was last modified.
    /// </summary>
    public DateTimeOffset? Modified { get; set; }

    /// <summary>
    /// How many days old this rate is.
    /// </summary>
    public int DaysOld { get; set; }

    /// <summary>
    /// Freshness status of the rate data.
    /// </summary>
    public string FreshnessStatus { get; set; } = string.Empty;

    /// <summary>
    /// Minutes since the last update.
    /// </summary>
    public int? MinutesSinceUpdate { get; set; }

    /// <summary>
    /// Human-readable update freshness.
    /// </summary>
    public string? UpdateFreshness { get; set; }

    /// <summary>
    /// Indicates if this rate is considered fresh (less than 2 days old).
    /// </summary>
    public bool IsFresh => DaysOld < 2;

    /// <summary>
    /// Indicates if this rate was recently updated (less than 60 minutes).
    /// </summary>
    public bool IsRecentlyUpdated => MinutesSinceUpdate.HasValue && MinutesSinceUpdate.Value < 60;
}
