namespace ExchangeRateUpdater.Core.Exceptions;

public class ExchangeRateException : Exception
{
    public ExchangeRateException(string message, Exception ex) : base(message, ex)
    {
    }
}