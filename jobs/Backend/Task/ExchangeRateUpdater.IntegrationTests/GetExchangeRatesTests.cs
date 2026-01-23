using System.Net;
using System.Net.Http.Json;
using ExchangeRateUpdater.Application.Models;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;

namespace ExchangeRateUpdater.IntegrationTests;

public sealed class GetExchangeRatesTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GetExchangeRatesTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetExchangeRates_WhenValidCurrenciesProvided_SuccessReturns200()
    {
        // Act
        var url = QueryHelpers.AddQueryString(
            "/exchange-rates",
            [
                new KeyValuePair<string, string?>("currencies", "usd"),
                new KeyValuePair<string, string?>("currencies", "eur")
            ]);
        
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<List<ExchangeRate>>();
        Assert.Equal([new("USD", "CZK", 3), new("EUR", "CZK", 20)], result);
    }

    [Fact]
    public async Task GetExchangeRates_WhenCurrenciesNotProvided_Returns400()
    {
        // Act
        var response = await _client.GetAsync("/exchange-rates");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetExchangeRates_WhenCurrenciesAreWrongFormat_Returns400()
    {
        // Act
        var url = QueryHelpers.AddQueryString(
            "/exchange-rates",
            [
                new KeyValuePair<string, string?>("currencies", "u")
            ]);
        
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}