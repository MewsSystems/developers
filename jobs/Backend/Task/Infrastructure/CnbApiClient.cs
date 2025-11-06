using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Constants;
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
            _logger.LogInformation(LogMessages.CnbApiClient.FetchingExchangeRates, _configuration.ApiUrl);

            var response = await _httpClient.GetAsync(_configuration.ApiUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation(LogMessages.CnbApiClient.FetchSuccessful);

            return content;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, LogMessages.CnbApiClient.HttpRequestFailed);
            throw new ExchangeRateProviderException(ExceptionMessages.CnbApiClient.NetworkError, ex);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, LogMessages.CnbApiClient.RequestTimedOut);
            throw new ExchangeRateProviderException(ExceptionMessages.CnbApiClient.Timeout, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.CnbApiClient.UnexpectedError);
            throw new ExchangeRateProviderException(ExceptionMessages.CnbApiClient.UnexpectedError, ex);
        }
    }
}
