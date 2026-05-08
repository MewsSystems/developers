using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure;

internal class HttpRequestHandler : IRequestHandler
{
    private HttpClient httpClient;

    public HttpRequestHandler(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public Task<Stream> GetStreamAsync(string requestUri)
    {
        return httpClient.GetStreamAsync(requestUri);
    }
}
