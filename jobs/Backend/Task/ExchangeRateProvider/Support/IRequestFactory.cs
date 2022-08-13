using System.Net.Http;

namespace ExchangeRateUpdater.Support;

public interface IRequestFactory
{
  HttpRequestMessage BuildRequest();
}