using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Common;
using Moq;

namespace ExchangeRateUpdater.Tests.Api;

public class ExchangeRatesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ExchangeRatesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetExchangeRates_ValidCurrencies_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/exchangerates?currencies=USD,EUR");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<ExchangeRateResponse>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data.Rates);
    }

    [Fact]
    public async Task GetExchangeRates_EmptyCurrencies_ShouldReturnBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/exchangerates?currencies=");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetExchangeRates_NoCurrenciesParameter_ShouldReturnBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/exchangerates");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
