namespace REST.Response.Models.Common;

/// <summary>
/// Base API response wrapper for all endpoints.
/// Provides a consistent response structure across the API.
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Optional message providing additional context about the operation.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Error details if the operation failed.
    /// </summary>
    public ErrorResponse? Error { get; set; }

    /// <summary>
    /// HTTP status code of the response.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Timestamp when the response was generated.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a successful response without data.
    /// </summary>
    public static ApiResponse Ok(string? message = null)
    {
        return new ApiResponse
        {
            Success = true,
            Message = message,
            StatusCode = 200
        };
    }

    /// <summary>
    /// Creates an error response.
    /// </summary>
    public static ApiResponse Fail(string message, int statusCode = 400, ErrorResponse? error = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            StatusCode = statusCode,
            Error = error ?? new ErrorResponse { Message = message }
        };
    }
}
