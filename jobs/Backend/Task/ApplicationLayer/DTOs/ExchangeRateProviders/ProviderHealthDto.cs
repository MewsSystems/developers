namespace ApplicationLayer.DTOs.ExchangeRateProviders;

/// <summary>
/// DTO for provider health status information.
/// </summary>
public class ProviderHealthDto
{
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsHealthy { get; set; }
    public int ConsecutiveFailures { get; set; }
    public DateTimeOffset? LastSuccessfulFetch { get; set; }
    public DateTimeOffset? LastFailedFetch { get; set; }
    public TimeSpan? TimeSinceLastSuccess { get; set; }
    public TimeSpan? TimeSinceLastFailure { get; set; }
}
