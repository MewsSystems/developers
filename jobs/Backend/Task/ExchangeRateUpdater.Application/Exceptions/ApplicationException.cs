using System;

namespace ExchangeRateUpdater.Application.Exceptions;

/// <summary>
/// Represents the base class for all application-specific exceptions.
/// </summary>
/// <param name="title">The title of the exception, providing a short description of the error.</param>
/// <param name="message">The detailed error message explaining the exception.</param>
/// <param name="innerException">An optional inner exception that provides additional context.</param>
public abstract class ApplicationException(string title, string message, Exception? innerException = null) : Exception(message, innerException)
{
    /// <summary>
    /// Gets the title of the exception.
    /// </summary>
    /// <value>
    /// A short, descriptive title for the exception.
    /// </value>
    public string Title { get; } = title;
}
