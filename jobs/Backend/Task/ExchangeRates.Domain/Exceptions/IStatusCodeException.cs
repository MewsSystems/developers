using System.Net;

namespace ExchangeRates.Domain.Exceptions;

public interface IStatusCodeException
{
    public HttpStatusCode StatusCode { get; }

    public string ErrorMessage { get; }
}
