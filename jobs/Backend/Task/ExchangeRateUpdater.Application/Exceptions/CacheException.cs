using System;

namespace ExchangeRateUpdater.Application.Exceptions;

/// <summary>
/// Represents an exception that occurs during caching operations.
/// </summary>
/// <param name="message">A detailed message describing the cache error.</param>
/// <param name="innerException">An optional inner exception providing additional context.</param>
public sealed class CacheException(string message, Exception? innerException = null) : ApplicationException("Cache error.", $"Caching error: {message}", innerException)
{
}
