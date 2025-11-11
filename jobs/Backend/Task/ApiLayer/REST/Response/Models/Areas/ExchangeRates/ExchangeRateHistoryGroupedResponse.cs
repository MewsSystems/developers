using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for exchange rate history with nested grouping.
/// Groups by Provider → Base Currency → Historical Rates.
/// Eliminates all duplicate provider and base currency information.
/// </summary>
public class ExchangeRateHistoryGroupedResponse
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
    /// Collection of base currencies with historical data.
    /// </summary>
    public List<HistoricalBaseCurrencyGroup> BaseCurrencies { get; set; } = new();

    /// <summary>
    /// Total number of base currencies.
    /// </summary>
    public int TotalBaseCurrencies => BaseCurrencies.Count;

    /// <summary>
    /// Total number of historical entries across all base currencies.
    /// </summary>
    public int TotalEntries => BaseCurrencies.Sum(bc => bc.TotalEntries);
}

/// <summary>
/// Group of historical exchange rates for a specific base currency within a provider.
/// </summary>
public class HistoricalBaseCurrencyGroup
{
    /// <summary>
    /// Base currency code for this group.
    /// </summary>
    public string BaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Collection of historical rate entries to various target currencies.
    /// </summary>
    public List<HistoricalTargetCurrencyRate> Rates { get; set; } = new();

    /// <summary>
    /// Total number of historical entries.
    /// </summary>
    public int TotalEntries => Rates.Count;
}

/// <summary>
/// Individual historical exchange rate entry to a target currency.
/// </summary>
public class HistoricalTargetCurrencyRate
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
