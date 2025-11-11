namespace ApplicationLayer.DTOs.ExchangeRates;

/// <summary>
/// DTO for exchange rate information.
/// </summary>
public class ExchangeRateDto
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public int BaseCurrencyId { get; set; }
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public int TargetCurrencyId { get; set; }
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public int Multiplier { get; set; }
    public decimal Rate { get; set; }
    public decimal EffectiveRate { get; set; }
    public DateOnly ValidDate { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
}
