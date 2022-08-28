namespace ExchangeRateUpdater.Exceptions;

public class RateRetrievalException : Exception
{
    public RateRetrievalException() { }

    public RateRetrievalException(string message)
        : base(message)
    {
    }
}