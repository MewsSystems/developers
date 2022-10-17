using ExchangeRateUpdater.Clients.Cnb.Parsers;
using ExchangeRateUpdater.Clients.Cnb.Responses;

namespace ExchangeRateUpdater.Clients.Cnb;

public class CnbClient : ICnbClient
{
    private readonly HttpClient _httpClient;
    private readonly ICnbClientResponseParser _parser;

    /// <summary>
    /// Constructs a <see cref="CnbClient"/>
    /// </summary>
    /// <param name="httpClient">The http client.</param>
    /// <param name="parser">The parser.</param>
    public CnbClient(HttpClient httpClient, ICnbClientResponseParser parser)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
    }

    /// <inheritdoc />
    public async Task<ExchangeRateResponse> GetExchangeRatesAsync()
    {
        var httpResponseMessage = await _httpClient.GetAsync(
            "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
        httpResponseMessage.EnsureSuccessStatusCode();

        var streamResponse = await httpResponseMessage.Content.ReadAsStreamAsync();
        return await ReadResponseAsync(streamResponse);
    }

    /// <summary>
    /// Reads response from stream
    /// </summary>
    /// <param name="streamResponse">The stream response.</param>
    private async Task<ExchangeRateResponse> ReadResponseAsync(Stream streamResponse)
    {
        var response = new ExchangeRateResponse();

        using var streamReader = new StreamReader(streamResponse);

        var info = await streamReader.ReadLineAsync();
        if (string.IsNullOrWhiteSpace(info))
        {
            throw new Exception("Information is missing.");
        }

        var currentDate = _parser.ExtractDate(info);
        if (currentDate == null)
        {
            throw new Exception("Invalid date.");
        }

        response.CurrentDate = currentDate.Value;

        var column = await streamReader.ReadLineAsync();

        if (string.IsNullOrWhiteSpace(column))
        {
            throw new Exception("Column is missing.");
        }

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