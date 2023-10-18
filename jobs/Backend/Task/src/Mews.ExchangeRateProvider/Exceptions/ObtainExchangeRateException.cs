namespace Mews.ExchangeRateProvider.Exceptions;

/// <summary>
/// An exception thrown when there is a failure to obtain exchange rate data from a provider
/// </summary>
public class ObtainExchangeRateException : Exception
{
    /// <summary>
    /// Constructs a new instance of the <see cref="ObtainExchangeRateException"/> class
    /// </summary>
    public ObtainExchangeRateException()
    {
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="ObtainExchangeRateException"/> class
    /// </summary>
    /// <param name="message">Message for the exception</param>
    public ObtainExchangeRateException(string message) : base(message)
    {
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="ObtainExchangeRateException"/> class
    /// </summary>
    /// <param name="message">Message for the exception</param>
    /// <param name="innerException">Optional inner exception</param>
    public ObtainExchangeRateException(string message, Exception? innerException) : base(message, innerException)
    {
    }
}