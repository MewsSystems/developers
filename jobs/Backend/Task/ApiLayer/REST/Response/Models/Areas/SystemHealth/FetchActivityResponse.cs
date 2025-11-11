using REST.Response.Models.Common;

namespace REST.Response.Models.Areas.SystemHealth;

/// <summary>
/// API response model for exchange rate fetch activity logs.
/// </summary>
public class FetchActivityResponse
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
    /// Fetch operation status (e.g., "Running", "Success", "Failed").
    /// </summary>
    public string Status { get; set; } = string.Empty;

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

    /// <summary>
    /// Indicates if the fetch was successful.
    /// </summary>
    public bool IsSuccessful => Status?.Equals("Success", StringComparison.OrdinalIgnoreCase) ?? false;

    /// <summary>
    /// Indicates if the fetch is still running.
    /// </summary>
    public bool IsRunning => Status?.Equals("Running", StringComparison.OrdinalIgnoreCase) ?? false;
}
