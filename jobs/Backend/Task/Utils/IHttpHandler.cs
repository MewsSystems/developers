using System.Net.Http;

namespace ExchangeRateUpdater.Utils;

public interface IHttpHandler
{
    /// <summary>
    /// Send a GET request to the specified Uri
    /// </summary>
    HttpResponseMessage Get(string url);
}