namespace ExchangeRateUpdater.Api;

/// <summary>
/// Error response model for API errors
/// </summary>
public record ErrorResponse
{
    /// <summary>
    /// Error message
    /// </summary>
    public required string Error { get; init; }

    /// <summary>
    /// Optional detailed error information
    /// </summary>
    public string? Details { get; init; }
}
