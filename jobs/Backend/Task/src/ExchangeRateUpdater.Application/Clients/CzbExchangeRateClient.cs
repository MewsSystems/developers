using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Application.ExchangeRates;
using ExchangeRateUpdater.Application.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace ExchangeRateUpdater.Application.Clients;

public class CzbExchangeRateClient : ICzbExchangeRateClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly CzbOptions _czbOptions;
    private readonly Currency _targetCurrency = new("CZK");

    public CzbExchangeRateClient(IHttpClientFactory httpClientFactory, IOptions<CzbOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _czbOptions = options.Value;
    }

    public async Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRate(string currency, DateTime? dateTime = null)
    {
        var url = new StringBuilder(_czbOptions.Url);

        url.Append($"/exrates/daily-currency-month?currency={currency}");

        if (dateTime is not null)
        {
            url.Append($"&url&yearMonth={dateTime.Value.ToString("yyyy-MM")}");
        }

        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync(url.ToString());

        if (!response.IsSuccessStatusCode)
        {
            return Result<IEnumerable<ExchangeRate>>.Failure("Failed to fetch exchange rate");
        }

        using var contentStream = await response.Content.ReadAsStreamAsync();

        var rateResponse = await JsonSerializer.DeserializeAsync<RateResponse>(contentStream);

        var sourceCurrency = new Currency(currency);

        List<ExchangeRate> rates = new();

        foreach (var r in rateResponse.Rates)
        {
            var exchangeRate = new ExchangeRate(
                sourceCurrency,
                _targetCurrency,
                r.Rate,
                r.Amount,
                r.ValidFor
            );

            rates.Add(exchangeRate);
        }

        return Result<IEnumerable<ExchangeRate>>.Success(rates);
    }
}
