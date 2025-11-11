namespace REST.Response.Models.Areas.Providers;

/// <summary>
/// API response model for exchange rate provider information.
/// </summary>
public class ProviderResponse
{
    /// <summary>
    /// Provider unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Provider name (e.g., "European Central Bank").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Provider code (e.g., "ECB", "CNB", "BNR").
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Provider API endpoint URL.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Base currency code for this provider.
    /// </summary>
    public string BaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the provider requires authentication.
    /// </summary>
    public bool RequiresAuthentication { get; set; }

    /// <summary>
    /// Indicates if the provider is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Timestamp of the last successful fetch (if any).
    /// </summary>
    public DateTimeOffset? LastSuccessfulFetch { get; set; }

    /// <summary>
    /// Timestamp of the last failed fetch (if any).
    /// </summary>
    public DateTimeOffset? LastFailedFetch { get; set; }

    /// <summary>
    /// Number of consecutive failures.
    /// </summary>
    public int ConsecutiveFailures { get; set; }

    /// <summary>
    /// Provider health status.
    /// </summary>
    public string HealthStatus => ConsecutiveFailures == 0 ? "Healthy" :
                                  ConsecutiveFailures < 5 ? "Warning" : "Critical";
}
