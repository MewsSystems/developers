using System;

namespace ExchangeRateUpdater.Exceptions;

public class NutSupportedRateProviderException : Exception
{
    /// <summary>
    /// Initializes a new instance of the Exception class.
    /// </summary>
    public NutSupportedRateProviderException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Exception class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NutSupportedRateProviderException(string message)
        : base(message)
    {
    }
}