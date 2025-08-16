using Data;

namespace Infrastructure;

public abstract class BaseHttpApiClient : IApiClient
{
    protected readonly ILog<BaseHttpApiClient> _logger;
    private readonly HttpClient _httpClient;

    public BaseHttpApiClient(IHttpClientFactory httpClientFactory, ILog<BaseHttpApiClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient(HttpClientName);
        _logger = logger;
    }

    public abstract string HttpClientName { get; }

    public async Task<T> ExecuteAsync<T>(BaseApiRequest<T> request)
    {
        request.SetUri();
        request.HttpRequest.RequestUri = request.Uri;

        var httpResponse = await _httpClient.SendAsync(request.HttpRequest);

        if (httpResponse.IsSuccessStatusCode)
        {
            return await request.ParseHttpResponse(httpResponse, _logger);
        }
        else
        {
            _logger.Error("ExecuteAsync", httpResponse);
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(responseString);
        }
    }
}
