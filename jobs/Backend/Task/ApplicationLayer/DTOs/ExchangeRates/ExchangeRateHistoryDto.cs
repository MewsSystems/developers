namespace ApplicationLayer.DTOs.ExchangeRates;

/// <summary>
/// DTO for exchange rate history over time.
/// </summary>
public class ExchangeRateHistoryDto
{
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string SourceCurrencyCode { get; set; } = string.Empty;
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public DateOnly ValidDate { get; set; }
    public decimal Rate { get; set; }
    public int Multiplier { get; set; }
    public decimal EffectiveRate { get; set; }
    public decimal? ChangeFromPrevious { get; set; }
    public decimal? ChangePercentage { get; set; }
}
