namespace ApplicationLayer.DTOs.ExchangeRates;

/// <summary>
/// DTO for current exchange rates with additional metadata.
/// </summary>
public class CurrentExchangeRateDto
{
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int Multiplier { get; set; }
    public decimal EffectiveRate { get; set; }
    public DateOnly ValidDate { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public DateTimeOffset LastUpdated { get; set; }
    public int DaysOld { get; set; }
}
