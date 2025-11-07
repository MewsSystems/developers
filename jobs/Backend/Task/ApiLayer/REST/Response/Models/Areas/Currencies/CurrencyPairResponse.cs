namespace REST.Response.Models.Areas.Currencies;

/// <summary>
/// API response model for currency pair availability information.
/// </summary>
public class CurrencyPairResponse
{
    /// <summary>
    /// Base currency code (ISO 4217).
    /// </summary>
    public string BaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Target currency code (ISO 4217).
    /// </summary>
    public string TargetCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Number of providers offering this currency pair.
    /// </summary>
    public int ProviderCount { get; set; }

    /// <summary>
    /// List of provider codes that offer this pair.
    /// </summary>
    public List<string> AvailableProviders { get; set; } = new();

    /// <summary>
    /// Date of the latest available rate for this pair (YYYY-MM-DD format).
    /// </summary>
    public string? LatestRateDate { get; set; }

    /// <summary>
    /// Latest exchange rate value for this pair.
    /// </summary>
    public decimal? LatestRate { get; set; }

    /// <summary>
    /// Currency pair string in standard format (e.g., "EUR/USD").
    /// </summary>
    public string CurrencyPair => $"{BaseCurrency}/{TargetCurrency}";

    /// <summary>
    /// Indicates if this pair is actively supported (has at least one provider).
    /// </summary>
    public bool IsSupported => ProviderCount > 0;
}
