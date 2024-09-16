using Polly;
using Polly.Retry;
using System.Text.Json;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseUrl;
    private readonly ILogger<ExchangeRateProvider> _logger;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _httpRetryPolicy;

    public ExchangeRateProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<ExchangeRateProvider> logger)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _baseUrl = configuration["ExchangeRateProvider:BaseUrl"] ?? throw new ArgumentNullException("Base URL not found in configuration.");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _httpRetryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2 * retryAttempt),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    _logger.LogWarning($"Request failed. Waiting {timespan} before next retry. Retry attempt {retryAttempt}");
                });
    }

    public async Task<ExchangeRate> GetExchangeRate(string currencyCode)
    {
        try
        {
            var response = await SendHttpRequestAsync(currencyCode);
            return await ProcessHttpResponseAsync(response, currencyCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred fetching exchange rates for {currencyCode}.");
            throw;
        }
    }

    private async Task<HttpResponseMessage> SendHttpRequestAsync(string currencyCode)
    {
        _logger.LogInformation($"Attempting to fetch exchange rates for {currencyCode}.");
        var client = _httpClientFactory.CreateClient();
        var requestUrl = $"{_baseUrl}?lang=EN";

        return await _httpRetryPolicy.ExecuteAsync(() => client.GetAsync(requestUrl));
    }

    private async Task<ExchangeRate> ProcessHttpResponseAsync(HttpResponseMessage response, string currencyCode)
    {
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning($"API request for {currencyCode} failed with status code: {response.StatusCode}.");
            throw new ExchangeRateApiException($"API request failed with status code: {response.StatusCode}", response.StatusCode);
        }

        var content = await response.Content.ReadAsStringAsync();
        _logger.LogInformation($"Successfully fetched exchange rates for {currencyCode}.");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var result = JsonSerializer.Deserialize<ExchangeRatesResponse>(content, options);

        var rate = result?.Rates?.FirstOrDefault(e => e.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
        if (rate == null)
        {
            _logger.LogWarning($"Currency code {currencyCode} not found.");
            throw new CurrencyNotFoundException(currencyCode);
        }

        return rate;
    }
}

public class ExchangeRatesResponse
{
    public List<ExchangeRate>? Rates { get; set; }
}
