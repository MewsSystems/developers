namespace ExchangeRateUpdater.Core.Exceptions;

public class ExchangeRateUpdaterException : Exception
{
    public ExchangeRateUpdaterException(string message) : base(message)
    {
    }
    
    public ExchangeRateUpdaterException(string message, Exception ex) : base(message, ex)
    {
    }
}