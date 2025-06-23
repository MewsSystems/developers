using System.Net;

namespace ExchangeRateUpdater.Infra;

public record HttpError(HttpStatusCode HttpStatusCode, string Error)
{
    
}