
using ExchangeRateProvider.Exceptions;

namespace ExchangeRateProvider.Implementations;

class CHttpClient(HttpClient httpClient) : IHttpClient
{
    private readonly HttpClient _httpClient = httpClient;

    public Uri BaseAddress
    {
        set => _httpClient.BaseAddress = value;
    }

    public TimeSpan Timeout
    {
        set => _httpClient.Timeout = value;
    }

    public int MaxRetries { get; set; }

    public Task<HttpResponseMessage> Get(string url)
    {
        return HandleHttpRetries(() => _httpClient.GetAsync(url));
    }

    private async Task<HttpResponseMessage> HandleHttpRetries(Func<Task<HttpResponseMessage>> httpClientFn)
    {
        for (int i = 0; i < MaxRetries; i++)
        {
            try
            {
                return await httpClientFn() ?? throw new UnexpectedException("http response is null");
            }
            catch (Exception)
            {
                if (i == MaxRetries - 1)
                    throw;
            }
        }
        throw new Exception();
    }
}