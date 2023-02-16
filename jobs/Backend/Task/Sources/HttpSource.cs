using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Sources;

public sealed class HttpSource : ISource
{
    private readonly string _requestUri;

    public HttpSource(string requestUri)
    {
        _requestUri = requestUri;
    }

    public async Task<string> GetContent()
    {
        using var httpClient = new HttpClient();
        return await httpClient.GetStringAsync(_requestUri);
    }

    public override string ToString()
    {
        return $"HttpClient: {_requestUri}";
    }
}
