namespace REST.Response.Models.Areas.ExchangeRates;

/// <summary>
/// API response model for exchange rate fetch operation results.
/// </summary>
public class FetchResultResponse
{
    /// <summary>
    /// Provider ID that performed the fetch.
    /// </summary>
    public int ProviderId { get; set; }

    /// <summary>
    /// Provider code (e.g., "ECB", "CNB").
    /// </summary>
    public string ProviderCode { get; set; } = string.Empty;

    /// <summary>
    /// Fetch operation status (e.g., "Success", "Failed", "Partial").
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Number of new rates imported.
    /// </summary>
    public int RatesImported { get; set; }

    /// <summary>
    /// Number of existing rates updated.
    /// </summary>
    public int RatesUpdated { get; set; }

    /// <summary>
    /// Total number of rates processed.
    /// </summary>
    public int TotalRatesProcessed { get; set; }

    /// <summary>
    /// Error message if the fetch failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// When the fetch operation completed.
    /// </summary>
    public DateTimeOffset CompletedAt { get; set; }

    /// <summary>
    /// Duration of the fetch operation in milliseconds.
    /// </summary>
    public int DurationMs { get; set; }

    /// <summary>
    /// Human-readable duration format.
    /// </summary>
    public string DurationFormatted
    {
        get
        {
            if (DurationMs < 1000) return $"{DurationMs}ms";
            if (DurationMs < 60000) return $"{DurationMs / 1000.0:F1}s";
            return $"{DurationMs / 60000.0:F1}min";
        }
    }

    /// <summary>
    /// Indicates if the fetch was successful.
    /// </summary>
    public bool IsSuccessful => Status?.Equals("Success", StringComparison.OrdinalIgnoreCase) ?? false;
}
