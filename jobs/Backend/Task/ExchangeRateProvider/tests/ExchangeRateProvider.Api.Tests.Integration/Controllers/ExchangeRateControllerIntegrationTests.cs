using ExchangeRateProvider.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace ExchangeRateProvider.Api.Tests.Integration.Controllers;

public class ExchangeRateControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ExchangeRateControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetByCurrency_WithValidCzkCurrency_ReturnsOkWithRates()
    {
        // Act
        var response = await _client.GetAsync("/v1/api/exchange-rates?baseCurrency=CZK");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var rates = await response.Content.ReadFromJsonAsync<List<ExchangeRateDto>>();
        rates.ShouldNotBeNull();
        rates.ShouldNotBeEmpty();
        rates.ShouldAllBe(r => r.BaseCurrency.Code == "CZK");
    }

    [Fact]
    public async Task GetByCurrency_WithCzkAndQuoteCurrencies_ReturnsFilteredRates()
    {
        // Act
        var response = await _client.GetAsync("/v1/api/exchange-rates?baseCurrency=CZK&quoteCurrencies=EUR&quoteCurrencies=USD");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var rates = await response.Content.ReadFromJsonAsync<List<ExchangeRateDto>>();
        rates.ShouldNotBeNull();
        rates.ShouldAllBe(r => r.QuoteCurrency.Code == "EUR" || r.QuoteCurrency.Code == "USD");
    }

    [Fact]
    public async Task GetByCurrency_WithInvalidBaseCurrency_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/v1/api/exchange-rates?baseCurrency=XXX");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetByCurrency_WithInvalidQuoteCurrency_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/v1/api/exchange-rates?baseCurrency=CZK&quoteCurrencies=INVALID");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetByCurrency_WithoutBaseCurrency_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/v1/api/exchange-rates");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}