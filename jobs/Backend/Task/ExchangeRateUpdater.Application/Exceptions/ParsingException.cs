using System;

namespace ExchangeRateUpdater.Application.Exceptions;

/// <summary>
/// Represents an exception that occurs when parsing an API response fails.
/// </summary>
/// <param name="message">A detailed message describing the parsing error.</param>
/// <param name="innerException">The inner exception that caused this parsing failure (optional).</param>
public sealed class ParsingException(string message, Exception? innerException = null) : ApplicationException("Parsing error.", message, innerException)
{
}
