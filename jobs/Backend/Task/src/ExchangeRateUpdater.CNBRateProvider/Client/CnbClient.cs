using System.Net.Http.Json;
using ExchangeRateUpdater.CNBRateProvider.Client.Dto;
using ExchangeRateUpdater.Domain.Models;
using FluentResults;

namespace ExchangeRateUpdater.CNBRateProvider.Client;

internal class CnbClient : ICnbClient
{
    private readonly HttpClient _httpClient;

    // TODO: change once CBN API starts supporting different source codes
    private readonly Currency _sourceCurrencyCode = new ("CZK");

    public CnbClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<ExchangeRate>>> GetDailyExchangeRate(DateTime dateTime, CancellationToken cancellationToken)
    {
        var dailyExchangeRateUri = new Uri($"cnbapi/exrates/daily?date={dateTime.Date:yyyy-MM-dd}&lang=EN", UriKind.Relative);

        var response = await _httpClient.GetAsync(dailyExchangeRateUri, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail($"Failed to obtain exchange rates. CNB responded with {(int)response.StatusCode} status code");
        }

        var responsePayload = await response.Content.ReadFromJsonAsync<ExchangeRateResponse>(cancellationToken: cancellationToken);
        if (responsePayload is not null)
        {
            var rates = responsePayload.Rates.Select(rate =>
                new ExchangeRate(new Currency(rate.CurrencyCode), _sourceCurrencyCode, rate.Rate / rate.Amount));
            return Result.Ok(rates);
        }

        return Result.Ok();
    }
}
