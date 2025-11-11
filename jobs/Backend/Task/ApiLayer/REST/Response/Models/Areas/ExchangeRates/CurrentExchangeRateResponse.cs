using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for current exchange rates.
/// This model controls exactly what the API consumers receive.
/// </summary>
public class CurrentExchangeRateResponse
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
    /// Provider information.
    /// </summary>
    public ProviderInfo Provider { get; set; } = new();

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
