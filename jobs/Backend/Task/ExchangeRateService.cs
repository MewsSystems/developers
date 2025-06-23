using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;

public class ExchangeRateService : IExchangeRateService
{
    private const string _ratesSourceUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRateService> _logger;

    public ExchangeRateService(HttpClient httpClient, ILogger<ExchangeRateService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string?> GetExchangeRatesData()
    {
        try
        {
            return await _httpClient.GetStringAsync(_ratesSourceUrl);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"HTTP request error: {ex.Message}");
            return null;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError($"Request timed out: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred: {ex.Message}");
            return null;
        }
    }
}
