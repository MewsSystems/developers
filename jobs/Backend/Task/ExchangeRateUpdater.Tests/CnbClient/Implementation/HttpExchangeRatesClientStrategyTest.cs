using System.Net;
using System.Text;
using System.Text.Json;
using ExchangeRateUpdater.Abstractions.Exceptions;
using ExchangeRateUpdater.CnbClient.Dtos;
using ExchangeRateUpdater.CnbClient.Implementation;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace ExchangeRateUpdater.Tests.CnbClient.Implementation;

[TestFixture]
[TestOf(typeof(HttpExchangeRatesClientStrategy))]
public class HttpExchangeRatesClientStrategyTest
{
    private IHttpClientFactory httpClientFactoryMock = null!;
    private IConfiguration configurationMock;
    private TestHandler handler = null!;
    private HttpClient httpClient = null!;
    private const string TestUrl = "https://test-url";

    [SetUp]
    public void SetUp()
    {
        configurationMock = Substitute.For<IConfiguration>();
        configurationMock["CnbClient:Url"].Returns(TestUrl);
    }

    [TearDown]
    public void TearDown()
    {
        httpClient.Dispose();
        handler.Dispose();
    }

    private void SetupHttpClient(HttpResponseMessage response)
    {
        handler = new TestHandler(response);
        httpClient = new HttpClient(handler, false);
        httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
        httpClientFactoryMock.CreateClient(Arg.Any<string>()).Returns(httpClient);
    }

    private class TestHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(response);
    }

    [Test]
    public async Task GetExchangeRates_ReturnsRates_WhenResponseIsValid()
    {
        // Arrange
        var dto = new ExchangeRatesResponseDto
        {
            Rates =
            [
                new ExchangeRateDto { CurrencyCode = "USD", Rate = 23.5m, Amount = 1 },
                new ExchangeRateDto { CurrencyCode = "EUR", Rate = 25.0m, Amount = 1 }
            ]
        };
        var json = JsonSerializer.Serialize(dto);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        SetupHttpClient(response);
        var client = new HttpExchangeRatesClientStrategy(httpClientFactoryMock, configurationMock);

        // Act
        var result = await client.GetExchangeRates();

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result.Any(r => r is { CurrencyCode: "USD", Rate: 23.5m }));
            Assert.That(result.Any(r => r is { CurrencyCode: "EUR", Rate: 25.0m }));
        });
    }

    [Test]
    public void GetExchangeRates_ThrowsNotFound_WhenResponseIsNull()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", Encoding.UTF8, "application/json")
        };
        SetupHttpClient(response);
        var client = new HttpExchangeRatesClientStrategy(httpClientFactoryMock, configurationMock);

        // Act & Assert
        Assert.ThrowsAsync<ExchangeRateNotFoundException>(() => client.GetExchangeRates());
    }

    [Test]
    public void GetExchangeRates_ThrowsHttpRequestException_WhenResponseIsNotSuccess()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        SetupHttpClient(response);
        var client = new HttpExchangeRatesClientStrategy(httpClientFactoryMock, configurationMock);

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(() => client.GetExchangeRates());
    }

    [Test]
    public void GetExchangeRates_ThrowsJsonException_WhenResponseIsMalformed()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("not a json", Encoding.UTF8, "application/json")
        };
        SetupHttpClient(response);
        var client = new HttpExchangeRatesClientStrategy(httpClientFactoryMock, configurationMock);

        // Act & Assert
        Assert.ThrowsAsync<JsonException>(() => client.GetExchangeRates());
    }
}