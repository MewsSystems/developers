using ExchangeRateUpdater.Services;
using Moq.Protected;
using Moq;
using System.Net;
using ExchangeRateUpdater.Models;
using System.Text.Json;

[TestFixture]
public class ExchangeRatesServiceTests
{
    private ExchangeRatesService _exchangeRatesService;
    private Mock<IHttpClientFactory> _httpClientFactory;
    private Mock<HttpMessageHandler> _handlerMock;

    [TearDown]
    public void Teardown()
    {
        _handlerMock.Reset();
        _httpClientFactory.Reset();
    }

    [Test]
    public void GetExchangeRatesAsync_ShouldNotReturnNull()
    {
        // Arrange
        var content = OkResponseCode();
        SetUpExchangeRateServiceAndMockClient(HttpStatusCode.OK, content);

        // Act
        var result = _exchangeRatesService.GetExchangeRatesAsync();

        // Assert
        Assert.IsNotNull(result);
    }

    [Test]
    public void GetExchangeRatesAsync_ThrowsException_WhenResponseFails()
    {
        // Arrange
        SetUpExchangeRateServiceAndMockClient(HttpStatusCode.NotFound, string.Empty);

        // Assert
        Assert.ThrowsAsync<Exception>(async () => await _exchangeRatesService.GetExchangeRatesAsync());
    }

    private void SetUpExchangeRateServiceAndMockClient(HttpStatusCode httpStatusCode, string content)
    {
        _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(content),
            })
            .Verifiable();

        _httpClientFactory = new Mock<IHttpClientFactory>();
        var httpClient = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("http://test.com/"),
        };
        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _exchangeRatesService = new ExchangeRatesService(_httpClientFactory.Object);
    }

    private string OkResponseCode()
    {
        var responseData = new ExchangeRatesResponseModel
        {
            ExchangeRates = new()
            {
                new()
                {
                    Country = "Australia",
                    Currency = "dollar",
                    CurrencyCode = "AUD",
                    Rate = 15.038
                },

            }

        };

        return JsonSerializer.Serialize(responseData);
    }

    private string NotFoundResponseCode()
    {
        var responseData = new ExchangeRatesResponseModel
        {
            ExchangeRates = null
        };

        return JsonSerializer.Serialize(responseData);
    }
}