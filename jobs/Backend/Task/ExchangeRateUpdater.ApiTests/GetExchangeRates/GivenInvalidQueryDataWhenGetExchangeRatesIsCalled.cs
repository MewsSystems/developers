using ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;
using FluentAssertions;
using System.Net;
using System.Text;
using ExchangeRateUpdater.ApiTests.Setup;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateProvider.ApiTests.GetExchangeRates;

[Collection("ExchangeRateUpdaterTests")]
public class GivenInvalidRequestWhenGetExchangeRatesIsCalled(ExchangeRateUpdaterApplicationFactory factory) : IAsyncLifetime
{
    private HttpResponseMessage _httpResponse = null!;
    private HttpClient _httpClient = null!;
    private DateTime _queryDate = DateTime.UtcNow;

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        await Task.Yield();
        _httpClient = factory.HttpClient;
    }

    [Theory]
    [InlineData("EURO", "Must be valid ISO code")]
    [InlineData(null, "'Currency Codes' must not be empty")]
    public async void Then400BadRequestResponseReturned(string currencyCode, string errorDetail)
    {
        var getExchangeRatesQuery = new GetExchangeRatesByCurrencyQuery(CurrencyCodes: [currencyCode], _queryDate);
        var json = JsonSerializer.Serialize(getExchangeRatesQuery);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpResponse = await _httpClient.PostAsync("/exchangeRates", content);
        _httpResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var problemDetails = await _httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails!.Detail.Should().Contain(errorDetail);
    }
}
