using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;

public class CnbExchangeRateService : ICnbExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CnbExchangeRateService> _logger;
    private readonly CnbApiOptions _options;
    private readonly ICnbResponseParser _parser;

    public CnbExchangeRateService(
        HttpClient httpClient,
        ILogger<CnbExchangeRateService> logger,
        IOptions<CnbApiOptions> options,
        ICnbResponseParser parser)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));

        _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
    }

    public async Task<Dictionary<string, decimal>> GetDailyRatesAsync(DateTime? date = null, CancellationToken cancellationToken = default)
    {
        var targetDate = date ?? DateTime.Today;
        var url = $"{_options.BaseUrl}?{CnbConstants.DateQueryParameter}={targetDate.ToString(CnbConstants.DateFormat)}";

        try
        {
            _logger.LogDebug("Fetching exchange rates from CNB for date {Date}", targetDate.ToString(CnbConstants.DateFormat));

            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var xmlContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Retrieved {Length} characters from CNB API", xmlContent.Length);

            return _parser.Parse(xmlContent, targetDate);
        }
        catch (OperationCanceledException ex) when (ex is TaskCanceledException)
        {
            _logger.LogError(ex, "Request to CNB timed out after {TimeoutSeconds} seconds", _options.TimeoutSeconds);
            throw new CnbApiException($"Request to CNB timed out after {_options.TimeoutSeconds} seconds", ex);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch rates from CNB");
            throw new CnbApiException($"Failed to retrieve exchange rates from CNB: {ex.Message}", ex);
        }
        catch (Exception ex) when (ex is not CnbApiException)
        {
            _logger.LogError(ex, "Unexpected error retrieving rates from CNB");
            throw new CnbApiException("Unexpected error retrieving exchange rates", ex);
        }
    }
}
