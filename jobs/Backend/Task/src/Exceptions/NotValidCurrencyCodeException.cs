using System;

namespace ExchangeRateUpdater.Exceptions;

public class NotValidCurrencyCodeException : Exception
{
    /// <summary>
    /// Initializes a new instance of the Exception class.
    /// </summary>
    public NotValidCurrencyCodeException(string message) : base(message)
    {
    }
}