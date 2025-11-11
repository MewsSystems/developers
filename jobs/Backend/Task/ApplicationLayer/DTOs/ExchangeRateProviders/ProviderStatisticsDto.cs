namespace ApplicationLayer.DTOs.ExchangeRateProviders;

/// <summary>
/// DTO for provider statistics and performance metrics.
/// </summary>
public class ProviderStatisticsDto
{
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public int TotalRatesProvided { get; set; }
    public int TotalFetchAttempts { get; set; }
    public int SuccessfulFetches { get; set; }
    public int FailedFetches { get; set; }
    public decimal SuccessRate { get; set; }
    public DateTimeOffset? FirstFetchDate { get; set; }
    public DateTimeOffset? LastFetchDate { get; set; }
    public TimeSpan? AverageFetchInterval { get; set; }
    public DateOnly? OldestRateDate { get; set; }
    public DateOnly? NewestRateDate { get; set; }
}
