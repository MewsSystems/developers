using System.Net.Http;

namespace ExchangeRateUpdater.Utils;

internal class HttpHandler : IHttpHandler
{
    private readonly HttpClient _client = new HttpClient();

    public HttpResponseMessage Get(string url)
    {
        return _client.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
    }
}