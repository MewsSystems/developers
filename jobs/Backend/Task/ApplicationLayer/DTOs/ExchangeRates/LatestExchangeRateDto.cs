namespace ApplicationLayer.DTOs.ExchangeRates;

/// <summary>
/// DTO for latest exchange rate with freshness information.
/// Used for queries that return the most recent rates across providers.
/// </summary>
public class LatestExchangeRateDto
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public int BaseCurrencyId { get; set; }
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public int TargetCurrencyId { get; set; }
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int Multiplier { get; set; }
    public decimal EffectiveRate { get; set; }
    public DateOnly ValidDate { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public int DaysOld { get; set; }
    public string FreshnessStatus { get; set; } = string.Empty;
    public int? MinutesSinceUpdate { get; set; }
    public string? UpdateFreshness { get; set; }
}
