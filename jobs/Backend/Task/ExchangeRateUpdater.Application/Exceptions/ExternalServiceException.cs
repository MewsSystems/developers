using System;

namespace ExchangeRateUpdater.Application.Exceptions;

/// <summary>
/// Represents an exception that occurs when an external API request fails.
/// </summary>
/// <param name="message">A message that describes the external service failure.</param>
/// <param name="innerException">An optional inner exception providing additional details.</param>
public sealed class ExternalServiceException(string message, Exception? innerException = null) : ApplicationException("External service error.", message, innerException)
{
}
