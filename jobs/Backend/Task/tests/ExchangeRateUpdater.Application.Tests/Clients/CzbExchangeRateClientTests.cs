using ExchangeRateUpdater.Application.Clients;
using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Application.ExchangeRates;
using ExchangeRateUpdater.Application.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Net;

namespace ExchangeRateUpdater.Application.Tests.Clients;

public class CzbExchangeRateClientTests
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly CzbOptions _czbOptions;
    private readonly CzbExchangeRateClient _czbExchangeRateClient;

    public CzbExchangeRateClientTests()
    {
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
        _czbOptions = new CzbOptions { Url = "https://www.example.com" };
        _czbExchangeRateClient = new CzbExchangeRateClient(_httpClientFactory, Options.Create(_czbOptions));
    }

    [Fact]
    public async Task GetExchangeRate_ShouldReturnExchangeRates_WhenRequestIsSuccessful()
    {
        // Arrange
        var currency = "USD";
        var dateTime = new DateTime(2022, 5, 1);
        var rateResponse = new RateResponse
        {
            Rates = new List<RateItem>
                {
                    new RateItem { Rate = 1.2m, Amount = 100, ValidFor = "2022-05-01" },
                    new RateItem { Rate = 1.5m, Amount = 150, ValidFor = "2022-05-01" }
                }
        };
     
        var handler = new MyMockHttpMessageHandler(HttpStatusCode.OK, rateResponse);

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri(_czbOptions.Url)
        };

        _httpClientFactory.CreateClient().Returns(client);

        // Act
        var result = await _czbExchangeRateClient.GetExchangeRate(currency, dateTime);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().ContainEquivalentOf(new ExchangeRate(new Currency(currency), new Currency("CZK"), 1.2m, 100, "2022-05-01"));
        result.Value.Should().ContainEquivalentOf(new ExchangeRate(new Currency(currency), new Currency("CZK"), 1.5m, 150, "2022-05-01"));
    }

    [Fact]
    public async Task GetExchangeRate_ShouldReturnFailure_WhenRequestFails()
    {
        // Arrange
        var currency = "USD";
        var dateTime = new DateTime(2022, 5, 1);

        var handler = new MyMockHttpMessageHandler(HttpStatusCode.BadRequest, null);

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri(_czbOptions.Url)
        };

        _httpClientFactory.CreateClient().Returns(client);

        // Act
        var result = await _czbExchangeRateClient.GetExchangeRate(currency, dateTime);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Failed to fetch exchange rate");
    }
}