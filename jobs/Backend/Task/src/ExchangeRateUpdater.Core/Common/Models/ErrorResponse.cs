namespace ExchangeRateUpdater.Core.Common.Models;

/// <summary>
/// Standard error response format
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// HTTP status code
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Error title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Detailed error message
    /// </summary>
    public string Detail { get; set; }

    /// <summary>
    /// Validation errors by field
    /// </summary>
    public Dictionary<string, string[]> Errors { get; set; }
}