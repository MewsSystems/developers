namespace ExchangeRateError;

/// <summary>
/// Basic exchange rate exception
/// </summary>
public class ExchangeRateException : Exception
{

    public ExchangeRateException() : base()
    {
    }

    public ExchangeRateException(string message) : base(message)
    {
    }

    public ExchangeRateException(string message, Exception inner) : base(message, inner)
    {
    }

}