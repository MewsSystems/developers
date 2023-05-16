using System.Net;

namespace ExchangeRateUpdater.Client.Exceptions;

public class ExchangeRateProviderException : Exception
{
    public ExchangeRateProviderException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
    
    public HttpStatusCode StatusCode { get; init; }
}