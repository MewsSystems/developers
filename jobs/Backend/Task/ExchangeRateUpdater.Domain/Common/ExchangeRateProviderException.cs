namespace ExchangeRateUpdater.Domain.Common;

/// <summary>
/// Custom exception for exchange rate service errors
/// </summary>
public class ExchangeRateProviderException : Exception
{
    public ExchangeRateProviderException(string message) : base(message) { }
    public ExchangeRateProviderException(string message, Exception innerException) : base(message, innerException) { }
}
