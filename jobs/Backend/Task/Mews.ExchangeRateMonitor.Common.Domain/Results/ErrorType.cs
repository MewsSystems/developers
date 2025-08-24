namespace Mews.ExchangeRateMonitor.Common.Domain.Results;

public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
    Canceled,
    Custom
}