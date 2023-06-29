using System.Net.Http.Json;
using ExchangeRateUpdater.CNBRateProvider.Client.Dto;
using ExchangeRateUpdater.Domain.Models;
using FluentResults;

namespace ExchangeRateUpdater.CNBRateProvider.Client;

internal class CnbClient : ICnbClient
{
    private readonly HttpClient _httpClient;

    // 29/06/2023 CBN API currently supports only exchange rates to CZK,
    // there is no endpoint to request exchange rate to a different currency
    private readonly Currency _sourceCurrencyCode = Currency.FromString("CZK");

    public CnbClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<ExchangeRate>>> GetDailyExchangeRateToCzk(DateTime dateTime, CancellationToken cancellationToken)
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
                new ExchangeRate(Currency.FromString(rate.CurrencyCode) , _sourceCurrencyCode, rate.Rate / rate.Amount));
            return Result.Ok(rates);
        }

        return Result.Ok();
    }
}
