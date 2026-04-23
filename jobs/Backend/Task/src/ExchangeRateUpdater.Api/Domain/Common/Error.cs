namespace ExchangeRateUpdater.Api.Domain.Common;

/// <summary>
/// Represents a domain error with areadable error code and message.
/// </summary>
public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
