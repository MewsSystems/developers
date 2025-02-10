namespace ExchangeRateUpdater.Application.Exceptions;

/// <summary>
/// Represents an exception that occurs when a requested resource is not found.
/// </summary>
/// <param name="message">An optional message describing the missing resource. Defaults to "Resource not found".</param>
public sealed class NotFoundException(string message = "Resource not found.") : ApplicationException("Not found.", message)
{
}
