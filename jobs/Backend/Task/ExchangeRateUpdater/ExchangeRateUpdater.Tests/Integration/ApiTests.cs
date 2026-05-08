using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using ExchangeRateUpdater.Application.DTOs;

namespace ExchangeRateUpdater.Tests.Integration;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllRates_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/api/exchangerates");
        response.EnsureSuccessStatusCode();
        
        var rates = await response.Content.ReadFromJsonAsync<RatesResponse>();
        Assert.NotNull(rates);
        Assert.True(rates.TotalCount > 20);
    }

    [Fact]
    public async Task GetInvalidCurrency_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/exchangerates/XXX");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
