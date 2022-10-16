using AutoMapper;
using ExchangeRateUpdater.Clients.Cnb.Parsers;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;

namespace ExchangeRateUpdater.Clients.Cnb;

public class CnbClient : IExchangeRateProviderClient
{
    private readonly HttpClient _httpClient;
    private readonly CnbClientResponseParser _parser;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructs a <see cref="CnbClient"/>
    /// </summary>
    /// <param name="httpClient">The http client.</param>
    /// <param name="parser">The parser.</param>
    /// <param name="mapper">The mapper.</param>
    public CnbClient(HttpClient httpClient, CnbClientResponseParser parser, IMapper mapper)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
    {
        var httpResponseMessage = await _httpClient.GetAsync(
            "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
        httpResponseMessage.EnsureSuccessStatusCode();

        var streamResponse = await httpResponseMessage.Content.ReadAsStreamAsync();
        var response = await ReadResponseAsync(streamResponse);
        return _mapper.Map<IEnumerable<ExchangeRate>>(response);
    }

    /// <summary>
    /// Reads response from stream
    /// </summary>
    /// <param name="streamResponse">The stream response.</param>
    private async Task<ExchangeRatesResponse> ReadResponseAsync(Stream streamResponse)
    {
        var response = new ExchangeRatesResponse();

        using var streamReader = new StreamReader(streamResponse);

        var info = await streamReader.ReadLineAsync();
        if (string.IsNullOrWhiteSpace(info))
        {
            throw new Exception("Information is missing.");
        }

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