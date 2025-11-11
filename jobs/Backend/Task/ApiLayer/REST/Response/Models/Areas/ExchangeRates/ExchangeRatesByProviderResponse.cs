using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for exchange rates grouped by provider.
/// Avoids duplicating provider information for each rate.
/// </summary>
public class ExchangeRatesByProviderResponse
{
    /// <summary>
    /// Provider information.
    /// </summary>
    public ProviderInfo Provider { get; set; } = new();

    /// <summary>
    /// Collection of exchange rates from this provider.
    /// </summary>
    public List<ExchangeRateItem> Rates { get; set; } = new();

    /// <summary>
    /// Total number of rates from this provider.
    /// </summary>
    public int TotalRates => Rates.Count;
}

/// <summary>
/// Individual rate item within a provider's rate collection.
/// </summary>
public class ExchangeRateItem
{
    /// <summary>
    /// Exchange rate unique identifier.
    /// </summary>
    public int Id { get; set; }

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
}
