using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.Providers;

/// <summary>
/// API response model for provider health status.
/// </summary>
public class ProviderHealthResponse
{
    /// <summary>
    /// Provider information.
    /// </summary>
    public ProviderInfo Provider { get; set; } = new();

    /// <summary>
    /// Indicates if the provider is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Overall health status (e.g., "Healthy", "Warning", "Critical", "Inactive").
    /// </summary>
    public string HealthStatus { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of the last successful fetch.
    /// </summary>
    public DateTimeOffset? LastSuccessfulFetch { get; set; }

    /// <summary>
    /// Timestamp of the last failed fetch.
    /// </summary>
    public DateTimeOffset? LastFailedFetch { get; set; }

    /// <summary>
    /// Number of consecutive failures.
    /// </summary>
    public int ConsecutiveFailures { get; set; }

    /// <summary>
    /// Total number of successful fetches.
    /// </summary>
    public int TotalSuccessfulFetches { get; set; }

    /// <summary>
    /// Total number of failed fetches.
    /// </summary>
    public int TotalFailedFetches { get; set; }

    /// <summary>
    /// Success rate percentage.
    /// </summary>
    public decimal SuccessRate
    {
        get
        {
            var total = TotalSuccessfulFetches + TotalFailedFetches;
            return total > 0 ? (decimal)TotalSuccessfulFetches / total * 100 : 0;
        }
    }

    /// <summary>
    /// Average fetch duration in milliseconds.
    /// </summary>
    public int? AverageFetchDurationMs { get; set; }

    /// <summary>
    /// Indicates if provider is healthy.
    /// </summary>
    public bool IsHealthy => ConsecutiveFailures == 0 && IsActive;

    /// <summary>
    /// Hours since last successful fetch.
    /// </summary>
    public double? HoursSinceLastSuccess => LastSuccessfulFetch.HasValue
        ? (DateTimeOffset.UtcNow - LastSuccessfulFetch.Value).TotalHours
        : null;
}
