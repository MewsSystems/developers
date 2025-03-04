using System.Net;
using System.Net.Http.Json;
using ExchangeRateUpdater.Dto.ExchangeRates;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExchangeRateUpdater.IntegrationTests.API;

public class TestExchangeRatesController
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public TestExchangeRatesController()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetExchangeRates_WhenNoQueryParameter_ThenBadRequest()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/v1/exchange-rates/CZK");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetExchangeRates_WhenEmptyList_ThenBadRequest()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/v1/exchange-rates/CZK?targetCurrencies=");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetExchangeRates_WhenNonValidCurrencyValue_ThenBadRequest()
    {
        string targetCurrencies = "AAAAA";

        HttpResponseMessage response =
            await _client.GetAsync($"/api/v1/exchange-rates/CZK?targetCurrencies={targetCurrencies}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetExchangeRates_WhenValidValues_ThenReturnsCorrectInformation()
    {
        string targetCurrencies = "EUR";

        HttpResponseMessage response =
            await _client.GetAsync($"/api/v1/exchange-rates/CZK?targetCurrencies={targetCurrencies}");
        List<ExchangeRate>? result = await response.Content.ReadFromJsonAsync<List<ExchangeRate>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("EUR", result[0].TargetCurrency.Code);
    }
}