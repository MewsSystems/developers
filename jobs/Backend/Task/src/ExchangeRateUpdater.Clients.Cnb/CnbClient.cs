using ExchangeRateUpdater.Clients.Cnb.Parsers;
using ExchangeRateUpdater.Clients.Cnb.Responses;

namespace ExchangeRateUpdater.Clients.Cnb;

public class CnbClient : ICnbClient
{
    private readonly HttpClient _httpClient;
    private readonly CnbClientResponseParser _parser;

    /// <summary>
    /// Constructs a <see cref="CnbClient"/>
    /// </summary>
    /// <param name="httpClient">The http client.</param>
    /// <param name="parser">The parser.</param>
    public CnbClient(HttpClient httpClient, CnbClientResponseParser parser)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
    }

    /// <inheritdoc />
    public async Task<ExchangeRatesResponse> GetExchangeRatesAsync()
    {
        var response = await _httpClient.GetAsync(
            "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
        response.EnsureSuccessStatusCode();

        var streamResponse = await response.Content.ReadAsStreamAsync();
        return await ReadResponseAsync(streamResponse);
    }

    /// <summary>
    /// Reads response from stream
    /// </summary>
    /// <param name="streamResponse">The stream response.</param>
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