using ExchangeRates.Api.Infrastructure.Clients.Cnb.Models;

namespace ExchangeRates.Api.Infrastructure.Clients.Cnb;

public interface ICnbHttpClient
{
    Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRatesAsync(DateTime date);
}

public class CnbHttpClient : ICnbHttpClient
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    private static readonly Currency _destinationCurrency = new("CZK");
    private const string _dateQueryParamenterFormat = "yyyy-MM-dd";

    private readonly HttpClient _httpClient;

    public CnbHttpClient(HttpClient httpClient) 
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRatesAsync(DateTime date)
    {
        var response = await _httpClient.GetAsync($"/cnbapi/exrates/daily?date={DateTime.UtcNow.ToString(_dateQueryParamenterFormat)}&lang=EN");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var cnbExchangeRates = JsonSerializer.Deserialize<CnbExchangeRates>(content, _jsonSerializerOptions);

        if (cnbExchangeRates is null
            || cnbExchangeRates.Rates is null
            || cnbExchangeRates.Rates.Any(x => x.Amount == 0))
        {
            return Result<IEnumerable<ExchangeRate>>.Error("Error getting exchange rates from source, bad expected data");
        }

        return Result<IEnumerable<ExchangeRate>>.Success(MapToExchangeRates(cnbExchangeRates.Rates));   
    }

    private static IEnumerable<ExchangeRate> MapToExchangeRates(IEnumerable<CnbExchangeRate> cnbExchangeRates)
    {
        return cnbExchangeRates!.Select(x => new ExchangeRate(new Currency(x.CurrencyCode), _destinationCurrency, x.Rate / x.Amount));
    }
}
