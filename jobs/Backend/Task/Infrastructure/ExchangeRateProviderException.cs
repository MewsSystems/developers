namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// Exception thrown when exchange rate provider operations fail.
/// </summary>
public class ExchangeRateProviderException : Exception
{
    public ExchangeRateProviderException(string message) : base(message)
    {
    }

    public ExchangeRateProviderException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
