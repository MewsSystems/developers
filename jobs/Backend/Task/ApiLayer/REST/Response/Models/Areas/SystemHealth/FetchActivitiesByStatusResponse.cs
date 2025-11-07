using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.SystemHealth;

/// <summary>
/// API response model for fetch activities grouped by status.
/// Organizes activities by their outcome (Success, Failed, Running).
/// </summary>
public class FetchActivitiesByStatusResponse
{
    /// <summary>
    /// Status value for this group (e.g., "Success", "Failed", "Running").
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Collection of fetch activity logs with this status.
    /// </summary>
    public List<FetchActivityWithProviderItem> Activities { get; set; } = new();

    /// <summary>
    /// Total number of activities with this status.
    /// </summary>
    public int TotalActivities => Activities.Count;
}

/// <summary>
/// Individual fetch activity item that includes provider information.
/// </summary>
public class FetchActivityWithProviderItem
{
    /// <summary>
    /// Fetch log unique identifier.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Provider information.
    /// </summary>
    public ProviderInfo Provider { get; set; } = new();

    /// <summary>
    /// When the fetch operation started.
    /// </summary>
    public DateTimeOffset FetchStarted { get; set; }

    /// <summary>
    /// When the fetch operation completed (if completed).
    /// </summary>
    public DateTimeOffset? FetchCompleted { get; set; }

    /// <summary>
    /// Number of rates imported (new).
    /// </summary>
    public int? RatesImported { get; set; }

    /// <summary>
    /// Number of rates updated (existing).
    /// </summary>
    public int? RatesUpdated { get; set; }

    /// <summary>
    /// Total number of rates processed.
    /// </summary>
    public int TotalRatesProcessed => (RatesImported ?? 0) + (RatesUpdated ?? 0);

    /// <summary>
    /// Error message if the fetch failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Duration of the fetch operation in milliseconds.
    /// </summary>
    public int? DurationMs { get; set; }

    /// <summary>
    /// Duration formatted as human-readable string.
    /// </summary>
    public string? DurationFormatted
    {
        get
        {
            if (!DurationMs.HasValue) return null;
            if (DurationMs < 1000) return $"{DurationMs}ms";
            if (DurationMs < 60000) return $"{DurationMs / 1000.0:F1}s";
            return $"{DurationMs / 60000.0:F1}min";
        }
    }
}
