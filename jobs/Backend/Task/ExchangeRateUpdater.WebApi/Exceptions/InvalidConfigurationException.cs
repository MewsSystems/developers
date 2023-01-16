namespace ExchangeRateUpdater.WebApi.Exceptions;

public class InvalidConfigurationException : Exception
{
    /// <summary>
    /// Exception that will be thrown when the configuration is invalid.
    /// </summary>
    /// <param name="message">Exception message that will be shown</param>
    public InvalidConfigurationException(string message) : base(message)
    {

    }
}