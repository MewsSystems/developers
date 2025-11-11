namespace DomainLayer.Common;

/// <summary>
/// Represents an error with a code and description.
/// Provides factory methods for common error types.
/// </summary>
public sealed record Error(string Code, string Description)
{
    /// <summary>
    /// Creates a validation error.
    /// </summary>
    public static Error Validation(string code, string description) =>
        new(code, description);

    /// <summary>
    /// Creates a not found error.
    /// </summary>
    public static Error NotFound(string code, string description) =>
        new(code, description);

    /// <summary>
    /// Creates a conflict error.
    /// </summary>
    public static Error Conflict(string code, string description) =>
        new(code, description);

    /// <summary>
    /// Creates an unauthorized error.
    /// </summary>
    public static Error Unauthorized(string code, string description) =>
        new(code, description);

    /// <summary>
    /// Creates a forbidden error.
    /// </summary>
    public static Error Forbidden(string code, string description) =>
        new(code, description);

    /// <summary>
    /// Creates a failure error (generic).
    /// </summary>
    public static Error Failure(string code, string description) =>
        new(code, description);

    /// <summary>
    /// Converts the error to a string representation.
    /// </summary>
    public override string ToString() => $"{Code}: {Description}";
}
