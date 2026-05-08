namespace ExchangeRateUpdater.Domain.Common;

public class ExchangeRateProviderException : Exception
{
    public ExchangeRateProviderException(string message) : base(message) { }
    public ExchangeRateProviderException(string message, Exception innerException) : base(message, innerException) { }
}
