namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for currency conversion results.
/// </summary>
public class ConvertCurrencyResponse
{
    /// <summary>
    /// Source currency code (e.g., "USD").
    /// </summary>
    public string SourceCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// Target currency code (e.g., "CZK").
    /// </summary>
    public string TargetCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// Original amount in source currency.
    /// </summary>
    public decimal SourceAmount { get; set; }

    /// <summary>
    /// Converted amount in target currency.
    /// </summary>
    public decimal TargetAmount { get; set; }

    /// <summary>
    /// Exchange rate used for conversion (effective rate per single unit).
    /// </summary>
    public decimal EffectiveRate { get; set; }

    /// <summary>
    /// Date for which the exchange rate is valid (YYYY-MM-DD).
    /// </summary>
    public string ValidDate { get; set; } = string.Empty;

    /// <summary>
    /// Provider that supplied the exchange rate.
    /// </summary>
    public string ProviderCode { get; set; } = string.Empty;

    /// <summary>
    /// When the rate was last updated.
    /// </summary>
    public DateTimeOffset RateLastUpdated { get; set; }

    /// <summary>
    /// Formatted conversion string (e.g., "100.00 USD = 2,433.50 CZK").
    /// </summary>
    public string FormattedConversion => $"{SourceAmount:N2} {SourceCurrencyCode} = {TargetAmount:N2} {TargetCurrencyCode}";
}
