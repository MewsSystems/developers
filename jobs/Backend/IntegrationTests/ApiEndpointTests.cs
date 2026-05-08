using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ExchangeRateUpdater.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExchangeRateUpdater.IntegrationTests;

public class ApiEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public ApiEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetExchangeRates_WithValidCurrencies_ReturnsOkWithRates()
    {
        var response = await _client.GetAsync("/api/exchange-rates?currencies=USD,EUR,GBP");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var rates = JsonSerializer.Deserialize<List<ExchangeRateResponse>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        rates.Should().NotBeNull();
        rates.Should().HaveCount(3);
        rates.Should().OnlyContain(r => r.TargetCurrency == "CZK");
        rates.Should().OnlyContain(r => r.Rate > 0);
        rates.Should().Contain(r => r.SourceCurrency == "USD");
        rates.Should().Contain(r => r.SourceCurrency == "EUR");
        rates.Should().Contain(r => r.SourceCurrency == "GBP");
    }

    [Fact]
    public async Task GetExchangeRates_WithSingleCurrency_ReturnsOkWithSingleRate()
    {
        var response = await _client.GetAsync("/api/exchange-rates?currencies=EUR");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rates = await response.Content.ReadFromJsonAsync<List<ExchangeRateResponse>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        rates.Should().NotBeNull();
        rates.Should().HaveCount(1);
        rates![0].SourceCurrency.Should().Be("EUR");
        rates[0].TargetCurrency.Should().Be("CZK");
        rates[0].Rate.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetExchangeRates_WithMissingCurrencies_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/exchange-rates");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Currency codes are required");
    }

    [Fact]
    public async Task GetExchangeRates_WithEmptyCurrencies_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/exchange-rates?currencies=");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetExchangeRates_WithInvalidCurrency_ReturnsOkWithEmptyList()
    {
        var response = await _client.GetAsync("/api/exchange-rates?currencies=INVALID,XXX");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rates = await response.Content.ReadFromJsonAsync<List<ExchangeRateResponse>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        rates.Should().NotBeNull();
        rates.Should().BeEmpty();
    }

    [Fact]
    public async Task PostExchangeRates_WithValidCurrencies_ReturnsOkWithRates()
    {
        var request = new ExchangeRateRequest
        {
            CurrencyCodes = new[] { "USD", "EUR" }
        };

        var response = await _client.PostAsJsonAsync("/api/exchange-rates", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rates = await response.Content.ReadFromJsonAsync<List<ExchangeRateResponse>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        rates.Should().NotBeNull();
        rates.Should().HaveCount(2);
        rates.Should().OnlyContain(r => r.TargetCurrency == "CZK");
        rates.Should().OnlyContain(r => r.Rate > 0);
    }

    [Fact]
    public async Task PostExchangeRates_WithEmptyCurrencies_ReturnsBadRequest()
    {
        var request = new ExchangeRateRequest
        {
            CurrencyCodes = Array.Empty<string>()
        };

        var response = await _client.PostAsJsonAsync("/api/exchange-rates", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostExchangeRates_WithNullCurrencies_ReturnsBadRequest()
    {
        var request = new { CurrencyCodes = (string[]?)null };

        var response = await _client.PostAsJsonAsync("/api/exchange-rates", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetSupportedCurrencies_ReturnsOkWithCurrencyList()
    {
        var response = await _client.GetAsync("/api/exchange-rates/supported");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(content);

        result.GetProperty("baseCurrency").GetString().Should().Be("CZK");
        result.GetProperty("supportedCurrencies").EnumerateArray().Should().NotBeEmpty();
        result.GetProperty("count").GetInt32().Should().BeGreaterThan(0);

        var currencies = result.GetProperty("supportedCurrencies").EnumerateArray()
            .Select(c => c.GetString()).ToList();

        currencies.Should().Contain("USD");
        currencies.Should().Contain("EUR");
        currencies.Should().OnlyHaveUniqueItems();
        currencies.Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task HealthCheck_ReturnsOkWithHealthyStatus()
    {
        var response = await _client.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        result.GetProperty("status").GetString().Should().Be("Healthy");
        result.GetProperty("service").GetString().Should().Be("Exchange Rate API");
        result.TryGetProperty("timestamp", out _).Should().BeTrue();
    }
}
