namespace ExchangeRateProvider.Infrastructure.Model;

public class CNBApiHttpClient : IApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpRetryPolicy _httpRetryPolicy;
    private readonly ILogger<CNBApiHttpClient> _logger;
    public CNBApiHttpClient( ILogger<CNBApiHttpClient> logger, HttpClient httpClient, IHttpRetryPolicy httpRetryPolicy )
    {
        (_logger, _httpClient, _httpRetryPolicy) = (logger, httpClient, httpRetryPolicy);
    }

    public async Task<IEnumerable<CNBApiExchangeRateRecord>> GetDailyExchangeRatesAsync()
    {
        CNBApiDailyExchangeRateResponse response = default;

        try
        {
            var resp = await _httpRetryPolicy.CNBHttpPolicy.ExecuteAsync( async _ =>
            {
                _logger.LogTrace( "CNB API Url: {url}", _httpClient.BaseAddress );
                return await _httpClient.GetAsync( string.Empty, HttpCompletionOption.ResponseHeadersRead );
            }, new Dictionary<string, object>() );

            resp.EnsureSuccessStatusCode();
            var data = await resp.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            response = JsonSerializer.Deserialize<CNBApiDailyExchangeRateResponse>( data, options );
            _logger.LogDebug( "Received rates for {curr} currencies from CNB API", response.Rates.Count() );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not fetch the exchange rates. Error: {0}", ex.Message);
        }

        return response?.Rates;
    }
}
