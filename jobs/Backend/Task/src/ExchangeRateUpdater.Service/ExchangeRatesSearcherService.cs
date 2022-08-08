using Domain.Entities;
using Domain.Ports;
using Serilog;

namespace ExchangeRatesSearcherService;

public class ExchangeRatesSearcherService : IExchangeRatesSearcher
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly CzechNationalBankApiSettings _czechNationalBankApiSettings;

    public ExchangeRatesSearcherService(HttpClient httpClient, ILogger logger, CzechNationalBankApiSettings czechNationalBankApiSettings)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _czechNationalBankApiSettings = czechNationalBankApiSettings ?? throw new ArgumentNullException(nameof(czechNationalBankApiSettings));
    }
    
    public async Task<IEnumerable<ExchangeRate>>  GetExchangeRates(DateTime date)
    {
        try
        {
            var formattedDate = date.ToString("dd.MM.yyyy");
            
            _logger.Information($"Getting exchange rates for date: {formattedDate}");

            var response = await _httpClient.GetAsync($"{_czechNationalBankApiSettings.ApiBaseUrl}?date={formattedDate}");
           
            var responseContent = await response.Content.ReadAsStringAsync();
            var validLines = StringResponseParser.Parse(responseContent);
            
            var mapper = new ExchangeRatesMapper(validLines, _logger, _czechNationalBankApiSettings);
            return mapper.Map();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting exchange rates");
            throw;
        }
    }
}