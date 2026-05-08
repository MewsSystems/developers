namespace ExchangeRateUpdater;

public class CnbApiException : Exception
{
    public CnbApiException(string message) : base(message)
    {
    }

    public CnbApiException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
