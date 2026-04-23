using System;

namespace ExchangeRateUpdater.Exceptions;

public sealed class ExchangeRateProviderException : Exception
{
    public ExchangeRateProviderException(string message)
        : base(message)
    {
    }

    public ExchangeRateProviderException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
