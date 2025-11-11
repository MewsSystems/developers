namespace ApplicationLayer.DTOs.ExchangeRates;

/// <summary>
/// DTO for exchange rate fetch operation results.
/// </summary>
public class FetchResultDto
{
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int RatesImported { get; set; }
    public int RatesUpdated { get; set; }
    public int TotalRatesProcessed { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset CompletedAt { get; set; }
    public TimeSpan Duration { get; set; }
}
