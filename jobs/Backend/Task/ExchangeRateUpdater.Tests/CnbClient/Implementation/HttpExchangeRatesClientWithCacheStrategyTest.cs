using ExchangeRateUpdater.Abstractions.Exceptions;
using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;
using ExchangeRateUpdater.CnbClient.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using ExchangeRateUpdater.CnbClient.Dtos;

namespace ExchangeRateUpdater.Tests.CnbClient.Implementation;

[TestFixture]
[TestOf(typeof(HttpExchangeRatesClientWithCacheStrategy))]
public class HttpExchangeRatesClientWithCacheStrategyTest
{
    private HttpExchangeRatesClientStrategy innerClient;
    private ICache<CurrencyValue> cacheMock;
    private ILogger<HttpExchangeRatesClientWithCacheStrategy> loggerMock;
    private HttpExchangeRatesClientWithCacheStrategy strategy;
    private IHttpClientFactory httpClientFactoryMock;
    private IConfiguration configurationMock;

    [SetUp]
    public void SetUp()
    {
        httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
        configurationMock = Substitute.For<IConfiguration>();
        configurationMock["CnbClient:Url"].Returns("http://test-url");
        innerClient = new HttpExchangeRatesClientStrategy(httpClientFactoryMock, configurationMock);

        cacheMock = Substitute.For<ICache<CurrencyValue>>();
        loggerMock = Substitute.For<ILogger<HttpExchangeRatesClientWithCacheStrategy>>();
        strategy = new HttpExchangeRatesClientWithCacheStrategy(innerClient, cacheMock, loggerMock);
    }

    [TearDown]
    public void TearDown()
    {
        cacheMock.ClearReceivedCalls();
        loggerMock.ClearReceivedCalls();
    }

    [Test]
    public void GetExchangeRates_ThrowsNotFound_WhenCacheEmptyAndInnerFails()
    {
        // Arrange
        cacheMock.IsEmpty().Returns(true);

        // Act & Assert
        Assert.ThrowsAsync<ExchangeRateNotFoundException>(() => strategy.GetExchangeRates());
        cacheMock.Received(1).IsEmpty();
    }

    [Test]
    public async Task GetExchangeRates_ReturnsFresh_WhenCacheEmptyAndInnerSucceeds()
    {
        // Arrange
        cacheMock.IsEmpty().Returns(true);

        var fresh = new List<CurrencyValue> {
            new() { CurrencyCode = "USD", Amount = 1, ValidFor = DateTime.Today.AddDays(1), Rate = 1.0m }
        };

        cacheMock.GetAll().Returns(fresh);

        // Mock HTTP response for innerClient
        var dto = new ExchangeRatesResponseDto
        {
            Rates =
            [
                new ExchangeRateDto
                {
                    CurrencyCode = "USD",
                    Amount = 1,
                    ValidFor = DateTime.Today.AddDays(1),
                    Rate = 1.0m
                }
            ]
        };
        var json = JsonSerializer.Serialize(dto);

        // Custom HttpMessageHandler to return the desired response
        var handler = new TestHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        });

        var httpClient = new HttpClient(handler);
        httpClientFactoryMock.CreateClient().Returns(httpClient);

        // Act
        var result = await strategy.GetExchangeRates();

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].CurrencyCode, Is.EqualTo(fresh[0].CurrencyCode));
        cacheMock.Received(1).IsEmpty();
        cacheMock.Received(1).GetAll();
        cacheMock.Received(1).Set("USD", Arg.Is<CurrencyValue>(cv =>
            cv.CurrencyCode == "USD" &&
            cv.Amount == 1 &&
            cv.ValidFor == DateTime.Today.AddDays(1) &&
            cv.Rate == 1.0m
        ));
    }

    [Test]
    public async Task GetExchangeRates_ReturnsCache_WhenCacheValid()
    {
        // Arrange
        cacheMock.IsEmpty().Returns(false);
        var valid = new List<CurrencyValue> {
            new CurrencyValue { CurrencyCode = "USD", Amount = 1, ValidFor = DateTime.Today.AddDays(1), Rate = 1.0m }
        };
        cacheMock.GetAll().Returns(valid);

        // Act
        var result = await strategy.GetExchangeRates();

        // Assert
        Assert.That(result, Is.EqualTo(valid));
        cacheMock.Received(1).IsEmpty();
        cacheMock.Received(2).GetAll();
    }

    [Test]
    public async Task GetExchangeRates_RefreshesCache_WhenExpiredAndInnerSucceeds()
    {
        // Arrange
        cacheMock.IsEmpty().Returns(false);
        var expired = new List<CurrencyValue> {
            new CurrencyValue { CurrencyCode = "USD", Amount = 1, ValidFor = DateTime.Today.AddDays(-1), Rate = 1.0m }
        };
        var fresh = new List<CurrencyValue> {
            new CurrencyValue { CurrencyCode = "USD", Amount = 1, ValidFor = DateTime.Today.AddDays(1), Rate = 2.0m }
        };
        cacheMock.GetAll().Returns(expired, fresh);

        // Act
        var result = await strategy.GetExchangeRates();

        // Assert
        Assert.That(result, Is.EqualTo(fresh));
        cacheMock.Received(1).IsEmpty();
        cacheMock.Received(2).GetAll();
    }

    [Test]
    public async Task GetExchangeRates_ReturnsExpiredAndLogs_WhenExpiredAndInnerFails()
    {
        // Arrange
        cacheMock.IsEmpty().Returns(false);
        var expired = new List<CurrencyValue> {
            new() { CurrencyCode = "USD", Amount = 1, ValidFor = DateTime.Today.AddDays(-1), Rate = 1.0m }
        };
        cacheMock.GetAll().Returns(expired);

        // Act
        var result = await strategy.GetExchangeRates();

        // Assert
        Assert.That(result, Is.EqualTo(expired));
        cacheMock.Received(1).IsEmpty();
        cacheMock.Received(2).GetAll();
        loggerMock.Received().Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    // Helper class for mocking HttpMessageHandler
    private class TestHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(response);
        }
    }
}