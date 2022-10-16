using ExchangeRateUpdater.Clients.Cnb.Parsers;
using ExchangeRateUpdater.Clients.Cnb.Responses;

namespace ExchangeRateUpdater.Clients.Cnb;

public class CnbClient : ICnbClient
{
    private readonly HttpClient _httpClient;
    private readonly CnbClientResponseParser _parser;

    public CnbClient(HttpClient httpClient, CnbClientResponseParser parser)
    {
        _httpClient = httpClient;
        _parser = parser;
    }

    public async Task<ExchangeRatesResponse> GetExchangeRatesAsync()
    {
        var response = await _httpClient.GetAsync(
            "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
        var streamResponse = await response.Content.ReadAsStreamAsync();
        return await ReadResponseAsync(streamResponse);
    }

    private async Task<ExchangeRatesResponse> ReadResponseAsync(Stream streamResponse)
    {
        var response = new ExchangeRatesResponse();

        using var streamReader = new StreamReader(streamResponse);

        while (await streamReader.ReadLineAsync() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var rate = _parser.ExtractExchangeRate(line);
            if (rate != null)
            {
                response.ExchangeRates.Add(rate);
            }
        }

        return response;
    }
}