namespace ExchangeRateUpdater.WebApi.Exceptions;

public class ServiceUnavailableException : Exception
{
    /// <summary>
    /// Exception that will be thrown when the exchange rates url is unavailable.
    /// </summary>
    /// <param name="message">Exception message that will be shown</param>
    public ServiceUnavailableException(string message) : base(message)
    {
        
    }
}