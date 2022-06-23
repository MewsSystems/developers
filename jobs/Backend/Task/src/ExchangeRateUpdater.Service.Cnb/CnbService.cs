using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Models.Entities;
using ExchangeRateUpdater.Service.Cnb.Mappers;
using Serilog;

namespace ExchangeRateUpdater.Service.Cnb;

public class CnbService : IExchangeRateUpdaterService
{
    private readonly HttpClient _httpClient;
    private readonly IExchangeRateServiceSettings _settings;
    private readonly ILogger _logger;

    public CnbService (HttpClient httpClient, IExchangeRateServiceSettings settings, ILogger logger) 
    {
        _httpClient = httpClient;
        _settings   = settings;
        _logger     = logger;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime date)
    {
        try
        {
            _logger.Information("Getting exchange rates for {SelectedDate}", $"{date:dd.MM.yyyy}");

            var response       = await _httpClient.GetAsync($"{_settings.BaseUrl}?date={date:dd.MM.yyyy}");
            var streamResponse = await response.Content.ReadAsStreamAsync();

            List<ExchangeRate> rates = await ReadStreamAsync(streamResponse);

            return rates;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            throw;
        }
    }

    private async Task<List<ExchangeRate>> ReadStreamAsync(Stream stream)
    {
        using var streamReader = new StreamReader(stream);

        var dateSeqInfo = await streamReader.ReadLineAsync();
        if (string.IsNullOrWhiteSpace(dateSeqInfo))
            throw new Exception("Incorrect message format, date-sequence information is missing.");

        var columnInfo = await streamReader.ReadLineAsync(); 
        if (string.IsNullOrWhiteSpace(columnInfo))
            throw new Exception("Incorrect message format, column information is missing.");

        var lines = new List<string>();

        string? line;
        while ((line = await streamReader.ReadLineAsync()) != null)
            if (!string.IsNullOrWhiteSpace(line))
                lines.Add(line);

        var mapper = new CnbServiceMapper(_logger,
                                          _settings.DefaultCurrency,
                                          _settings.MappingDelimiter,
                                          _settings.MappingDecimalSeparator,
                                          _settings.ThrowExceptionOnMappingErrors);

        return mapper.Map(columnInfo, lines);
    }
}