namespace ApplicationLayer.DTOs.SystemHealth;

/// <summary>
/// DTO for recent fetch activity information.
/// </summary>
public class FetchActivityDto
{
    public long LogId { get; set; }
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? RatesImported { get; set; }
    public int? RatesUpdated { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public TimeSpan? Duration { get; set; }
}
