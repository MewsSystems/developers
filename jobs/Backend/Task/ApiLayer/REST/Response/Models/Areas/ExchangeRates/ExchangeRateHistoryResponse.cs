using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for exchange rate history over time.
/// </summary>
public class ExchangeRateHistoryResponse
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
