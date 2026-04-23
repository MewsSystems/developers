namespace ExchangeRateUpdater.Api.Domain.Common;

/// <summary>
/// Classifies the kind of error for proper HTTP status code mapping.
/// </summary>
public enum ErrorType
{
    Failure,
    Validation,
    NotFound,
    Unavailable
}
