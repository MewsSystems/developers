using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Clients;

/// <summary>
/// HTTP client for fetching Czech National Bank exchange rate data.
/// </summary>
sealed internal class CzechNationalBankHttpClient : ICzechNationalBankClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CzechNationalBankHttpClient> _logger;
    private readonly ProviderOptions _options;

    public CzechNationalBankHttpClient(
        HttpClient httpClient,
        ProviderOptions options,
        ILogger<CzechNationalBankHttpClient> logger)
    {
        _httpClient = httpClient;
        _options = options;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_options.UserAgent);
    }

    public async Task<string> GetDailyRatesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Fetching daily exchange rates from CNB: {Url}",
            new Uri(_httpClient.BaseAddress!, _options.DailyRatesPath)
        );

        try
        {
            var response = await _httpClient.GetAsync(_options.DailyRatesPath, cancellationToken);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation(
                "Successfully retrieved exchange rates data ({Size} bytes)",
                content.Length
            );

            _logger.LogDebug("Response content preview: {Preview}",
                content.Length > 200 ? content.Substring(0, 200) + "..." : content
            );

            return content;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed while fetching exchange rates from CNB");
            throw;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Request timed out while fetching exchange rates from CNB");
            throw;
        }
    }
}
