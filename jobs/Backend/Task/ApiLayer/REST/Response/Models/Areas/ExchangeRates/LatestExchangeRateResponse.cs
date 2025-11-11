using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for latest exchange rates with freshness indicators.
/// </summary>
public class LatestExchangeRateResponse
{
    /// <summary>
    /// Exchange rate unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Provider information.
    /// </summary>
    public ProviderInfo Provider { get; set; } = new();

    /// <summary>
    /// Currency pair for this exchange rate.
    /// </summary>
    public CurrencyPair CurrencyPair { get; set; } = new();

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
    /// Freshness status of the rate data (e.g., "Fresh", "Stale", "Old").
    /// </summary>
    public string FreshnessStatus { get; set; } = string.Empty;

    /// <summary>
    /// Minutes since the last update.
    /// </summary>
    public int? MinutesSinceUpdate { get; set; }

    /// <summary>
    /// Human-readable update freshness (e.g., "Updated 5 minutes ago").
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
