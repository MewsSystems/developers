namespace ExchangeRateUpdater.Models.Errors;

/// <summary>
/// Represents the type of an error.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Indicates a validation error.
    /// </summary>
    ValidationError,

    /// <summary>
    /// Indicates an API error.
    /// </summary>
    ApiError
}