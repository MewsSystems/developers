using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Infrastructure.Dtos;
using ExchangeRateUpdater.Infrastructure.ExchangeRateVendors;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

namespace ExchangeRateUpdater.Infrastructure.Tests;

public class CurrencyApiExchangeRateVendorTests
{
    private static CurrencyApiExchangeRateVendor CreateSut(MockHttpMessageHandler mock, Uri baseUri, string apiKey)
    {
        var client = new HttpClient(mock)
        {
            BaseAddress = baseUri
        };
        client.DefaultRequestHeaders.Add("apiKey", apiKey);
        var logger = NullLogger<CurrencyApiExchangeRateVendor>.Instance;
        return new CurrencyApiExchangeRateVendor(client, logger);
    }

    [Fact]
    public async Task GetExchangeRates_Success_ParsesResponse()
    {
        // Arrange
        var baseUri = new Uri("https://api.currencyapi.com/v3/");
        
        var jsonResponse =  File.ReadAllText("Mocks/ExchangeRates.json");
        var mock = new MockHttpMessageHandler();
        mock.When(HttpMethod.Get, baseUri + "latest?base_currency=CZK")
            .WithHeaders("apiKey", "TEST_KEY")
            .Respond("application/json", jsonResponse);

        var sut = CreateSut(mock, baseUri, "TEST_KEY");

        // Act
        var result = await sut.GetExchangeRates("CZK");

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.SourceCurrency.ToString() == "CZK" && r.TargetCurrency.ToString() == "USD" && r.Value == 0.043m);
        result.Should().Contain(r => r.SourceCurrency.ToString() == "CZK" && r.TargetCurrency.ToString() == "EUR" && r.Value == 0.039m);
    }

    [Fact]
    public async Task GetExchangeRates_NonSuccess_ReturnsEmpty()
    {
        // Arrange
        var baseUri = new Uri("https://api.currencyapi.com/v3/");
        var mock = new MockHttpMessageHandler();
        mock.When(HttpMethod.Get, baseUri + "latest?base_currency=CZK")
            .Respond(HttpStatusCode.InternalServerError);
        var sut = CreateSut(mock, baseUri, "KEY");

        // Act
        var result = await sut.GetExchangeRates("CZK");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetExchangeRates_HandlerThrows_ReturnsEmpty()
    {
        // Arrange
        var baseUri = new Uri("https://api.currencyapi.com/v3/");
        var mock = new MockHttpMessageHandler();
        mock.When(HttpMethod.Get, baseUri + "latest?base_currency=CZK")
            .Throw(new HttpRequestException("something happened"));
        var sut = CreateSut(mock, baseUri, "KEY");

        // Act
        var result = await sut.GetExchangeRates("CZK");

        // Assert
        result.Should().BeEmpty();
    }
}
