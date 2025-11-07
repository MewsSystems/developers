namespace REST.Response.Models.Areas.SystemHealth;

/// <summary>
/// API response model for overall system health status.
/// </summary>
public class HealthCheckResponse
{
    /// <summary>
    /// Overall system status (e.g., "Healthy", "Degraded", "Unhealthy").
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of the health check.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Total number of active providers.
    /// </summary>
    public int ActiveProviders { get; set; }

    /// <summary>
    /// Total number of inactive providers.
    /// </summary>
    public int InactiveProviders { get; set; }

    /// <summary>
    /// Number of providers with recent failures.
    /// </summary>
    public int ProvidersWithFailures { get; set; }

    /// <summary>
    /// Total number of exchange rates in the system.
    /// </summary>
    public int TotalExchangeRates { get; set; }

    /// <summary>
    /// Timestamp of the most recent rate update.
    /// </summary>
    public DateTimeOffset? MostRecentRateUpdate { get; set; }

    /// <summary>
    /// Number of errors in the last 24 hours.
    /// </summary>
    public int RecentErrorCount { get; set; }

    /// <summary>
    /// Database status (e.g., "Connected", "Disconnected").
    /// </summary>
    public string DatabaseStatus { get; set; } = "Unknown";

    /// <summary>
    /// Background jobs status (e.g., "Running", "Stopped").
    /// </summary>
    public string BackgroundJobsStatus { get; set; } = "Unknown";

    /// <summary>
    /// Indicates if the system is healthy.
    /// </summary>
    public bool IsHealthy => Status?.Equals("Healthy", StringComparison.OrdinalIgnoreCase) ?? false;
}
