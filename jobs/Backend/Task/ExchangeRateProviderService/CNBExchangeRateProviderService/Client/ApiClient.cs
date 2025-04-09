using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;

namespace ExchangeRateProviderService.CNBExchangeRateProviderService.Client;

internal interface IApiClient
{
    Task<string> GetDailyRatesJson();
}

internal class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ApiClientOptions _options;
    private readonly ILogger<ApiClient> _logger;

    public ApiClient(
        HttpClient httpClient, IOptions<ApiClientOptions> options, ILogger<ApiClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> GetDailyRatesJson()
    {
        if (string.IsNullOrEmpty(_options.Scheme)
            || string.IsNullOrEmpty(_options.Host)
            || string.IsNullOrEmpty(_options.DailyRatesEndpoint))
        {
            return string.Empty;
        }

        var uriBuilder = new UriBuilder
        {
            Scheme = _options.Scheme,
            Host = _options.Host,
            Path = _options.DailyRatesEndpoint,
        };

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, uriBuilder.ToString());

        try
        {
            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"_Http request to CNB API returned code: {response.StatusCode}");
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"_Http request to CNB API failed with exception: {ex.Message}");
            return string.Empty;
        }
    }
}
