using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for current exchange rates with nested grouping.
/// Groups by Provider → Base Currency → Target Currencies.
/// Eliminates all duplicate provider and base currency information.
/// </summary>
public class CurrentExchangeRatesGroupedResponse
{
    /// <summary>
    /// Provider information.
    /// </summary>
    public ProviderInfo Provider { get; set; } = new();

    /// <summary>
    /// Collection of base currencies offered by this provider.
    /// </summary>
    public List<CurrentBaseCurrencyGroup> BaseCurrencies { get; set; } = new();

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
/// Group of current exchange rates for a specific base currency within a provider.
/// </summary>
public class CurrentBaseCurrencyGroup
{
    /// <summary>
    /// Base currency code for this group.
    /// </summary>
    public string BaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Collection of current rates to various target currencies.
    /// </summary>
    public List<CurrentTargetCurrencyRate> Rates { get; set; } = new();

    /// <summary>
    /// Total number of target currencies available from this base.
    /// </summary>
    public int TotalTargetCurrencies => Rates.Count;
}

/// <summary>
/// Individual current exchange rate to a target currency.
/// </summary>
public class CurrentTargetCurrencyRate
{
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
