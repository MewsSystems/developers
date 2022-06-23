using ExchangeRateUpdater.Models.Entities;
using ExchangeRateUpdater.Service.Cnb.Mappers;

namespace ExchangeRateUpdater.Service.Cnb;

public class CnbService
{
    private readonly HttpClient _httpClient;
    private readonly string _url;
    private readonly string _targetCurrency;
    private readonly string _delimiter;
    private readonly string _decimalSeparator;
    private readonly bool _throwExceptionOnErrors;

    public CnbService (HttpClient httpClient, string url, string targetCurrency, string delimiter, string decimalSeparator, bool throwExceptionOnMappingErrors)
    {
        _httpClient             = httpClient;
        _url                    = url;
        _targetCurrency         = targetCurrency;
        _delimiter              = delimiter;
        _decimalSeparator       = decimalSeparator;
        _throwExceptionOnErrors = throwExceptionOnMappingErrors;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime date)
    {
        var response       = await _httpClient.GetAsync($"{_url}?date={date:dd.MM.yyyy}");
        var streamResponse = await response.Content.ReadAsStreamAsync();

        List<ExchangeRate> rates = await ReadStreamAsync(streamResponse);

        return rates;
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

        var mapper = new CnbServiceMapper(_targetCurrency,
                                          _delimiter,
                                          _decimalSeparator,
                                          _throwExceptionOnErrors);

        return mapper.Map(columnInfo, lines);
    }
}