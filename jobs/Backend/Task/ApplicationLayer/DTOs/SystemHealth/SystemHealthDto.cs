namespace ApplicationLayer.DTOs.SystemHealth;

/// <summary>
/// DTO for system health dashboard information.
/// </summary>
public class SystemHealthDto
{
    public int TotalProviders { get; set; }
    public int ActiveProviders { get; set; }
    public int QuarantinedProviders { get; set; }
    public int TotalCurrencies { get; set; }
    public int TotalExchangeRates { get; set; }
    public DateOnly? LatestRateDate { get; set; }
    public DateOnly? OldestRateDate { get; set; }
    public int TotalFetchesToday { get; set; }
    public int SuccessfulFetchesToday { get; set; }
    public int FailedFetchesToday { get; set; }
    public decimal SuccessRateToday { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}
