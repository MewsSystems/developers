using System.Net;

public class CurrencyNotFoundException : Exception
{
    public string CurrencyCode { get; }

    public CurrencyNotFoundException(string currencyCode)
        : base($"Currency code {currencyCode} not found.")
    {
        CurrencyCode = currencyCode;
    }
}

public class ExchangeRateApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ExchangeRateApiException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
