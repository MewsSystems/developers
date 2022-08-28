namespace ExchangeRateUpdater.Exceptions;

public class RateParseException : Exception
{
    public RateParseException() { }

    public RateParseException(string message)
        : base(message)
    {
    }
}