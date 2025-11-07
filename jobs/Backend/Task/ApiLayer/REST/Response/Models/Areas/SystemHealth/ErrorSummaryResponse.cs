namespace REST.Response.Models.Areas.SystemHealth;

/// <summary>
/// API response model for error log summaries.
/// </summary>
public class ErrorSummaryResponse
{
    /// <summary>
    /// Error log unique identifier.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// When the error occurred.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// Error severity level (e.g., "Info", "Warning", "Error", "Critical").
    /// </summary>
    public string Severity { get; set; } = string.Empty;

    /// <summary>
    /// Source component or service where the error occurred.
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Exception details (if available).
    /// </summary>
    public string? Exception { get; set; }

    /// <summary>
    /// Related user ID (if applicable).
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// Indicates if this is a critical error.
    /// </summary>
    public bool IsCritical => Severity?.Equals("Critical", StringComparison.OrdinalIgnoreCase) ?? false;

    /// <summary>
    /// Indicates if this error occurred in the last hour.
    /// </summary>
    public bool IsRecent => (DateTimeOffset.UtcNow - Timestamp).TotalHours < 1;
}
