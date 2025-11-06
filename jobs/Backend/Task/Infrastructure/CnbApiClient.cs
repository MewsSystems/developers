using ExchangeRateUpdater.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// HTTP client for interacting with the Czech National Bank API.
/// </summary>
public class CnbApiClient : ICnbApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CnbApiClient> _logger;
    private readonly CnbExchangeRateConfiguration _configuration;

    public CnbApiClient(
        HttpClient httpClient,
        ILogger<CnbApiClient> logger,
        IOptions<CnbExchangeRateConfiguration> configuration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));

        _httpClient.Timeout = TimeSpan.FromSeconds(_configuration.TimeoutSeconds);
    }

    public async Task<string> FetchExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching exchange rates from CNB API: {ApiUrl}", _configuration.ApiUrl);

            var response = await _httpClient.GetAsync(_configuration.ApiUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation("Successfully fetched exchange rates from CNB API");

            return content;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed while fetching exchange rates from CNB");
            throw new ExchangeRateProviderException("Failed to fetch exchange rates from CNB API due to network error", ex);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Request to CNB API timed out");
            throw new ExchangeRateProviderException("Request to CNB API timed out", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching exchange rates from CNB");
            throw new ExchangeRateProviderException("Unexpected error occurred while fetching exchange rates", ex);
        }
    }
}
