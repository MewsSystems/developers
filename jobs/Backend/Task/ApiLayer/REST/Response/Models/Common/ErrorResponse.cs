namespace REST.Response.Models.Common;

/// <summary>
/// Structured error information for failed operations.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Human-readable error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Optional error code for client-side error handling.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Field-specific validation errors.
    /// </summary>
    public Dictionary<string, string[]>? ValidationErrors { get; set; }

    /// <summary>
    /// Additional error details (for development/debugging).
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Stack trace (only included in development environment).
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// Creates a simple error response.
    /// </summary>
    public static ErrorResponse Create(string message, string? code = null)
    {
        return new ErrorResponse
        {
            Message = message,
            Code = code
        };
    }

    /// <summary>
    /// Creates an error response with validation errors.
    /// </summary>
    public static ErrorResponse Validation(string message, Dictionary<string, string[]> validationErrors)
    {
        return new ErrorResponse
        {
            Message = message,
            Code = "VALIDATION_ERROR",
            ValidationErrors = validationErrors
        };
    }

    /// <summary>
    /// Creates an error response from an exception.
    /// </summary>
    public static ErrorResponse FromException(Exception exception, bool includeDetails = false)
    {
        var error = new ErrorResponse
        {
            Message = exception.Message,
            Code = exception.GetType().Name
        };

        if (includeDetails)
        {
            error.Details = exception.ToString();
            error.StackTrace = exception.StackTrace;
        }

        return error;
    }
}
