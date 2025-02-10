namespace ExchangeRateUpdater.API.Models;

/// <summary>
/// Represents an error response returned by the API.
/// </summary>
public class ApiErrorResponse
{
    /// <summary>
    /// Gets or sets the title of the error response.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the HTTP status code associated with the error.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Gets or sets the detailed error message.
    /// </summary>
    public string Detail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the dictionary of validation errors, if any.
    /// </summary>
    public IReadOnlyDictionary<string, string[]>? Errors { get; set; }
}
