using System.Net;
using System.Net.Http.Json;
using ExchangeRateUpdater.Models.API;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests.Services;

[TestFixture]
public class ExchangeRateServiceTests
{
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private HttpClient _httpClient;
    private Mock<IConfiguration> _configurationMock;
    private Mock<ILogger<ExchangeRateService>> _loggerMock;
    private ExchangeRateService _service;
    private FakeTimeProvider _fakeTimeProvider;

    [SetUp]
    public void SetUp()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _loggerMock = new Mock<ILogger<ExchangeRateService>>();
        _configurationMock = new Mock<IConfiguration>();
        _fakeTimeProvider = new FakeTimeProvider();

        var inMemoryConfiguration = new Dictionary<string, string>
        {
            { "BaseApiUrl", "https://api.example.com" }
        };
            
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemoryConfiguration)
            .Build();

        _service = new ExchangeRateService(_httpClient, configuration, _fakeTimeProvider, _loggerMock.Object);
    }
        
    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }

    [Test]
    public async Task GetExchangeRatesAsync_SuccessfulResponse_ReturnsExchangeRatesResponseModel()
    {
        // Arrange
        var expectedResponse = new ExchangeRatesResponseModel
        {
            Rates =
            [
                new ExchangeRateResponseModel
                {
                    Amount = 1,
                    CurrencyCode = "USD",
                    Rate = 25.5m,
                    ValidFor = new DateTime(2024,10,10),
                    Currency = "Dollar"
                }
            ]
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedResponse)
            });

        // Act
        var result = await _service.GetExchangeRatesAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Test]
    public void GetExchangeRatesAsync_UnsuccessfulResponse_ThrowsHttpRequestException_AndLogsError()
    {
        // Arrange
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                ReasonPhrase = "Bad Request"
            });

        const string expectedLogMessage = 
            "Call to https://api.example.com/exrates/daily?date=2024-10-10&lang=EN was unsuccessful (400 - Bad Request)";

        // Act
        var exception = Assert.ThrowsAsync<HttpRequestException>(() => _service.GetExchangeRatesAsync(new DateTime(2024, 10,10)));

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Is.EqualTo("Error fetching exchange rates."));
            _loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == expectedLogMessage),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        });
    }

    [Test]
    public void GetExchangeRatesAsync_NullContent_ThrowsHttpRequestException()
    {
        // Arrange
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = null
            });

        // Act
        var exception = Assert.ThrowsAsync<HttpRequestException>(() => _service.GetExchangeRatesAsync());

        // Assert
        Assert.That(exception.Message, Is.EqualTo("Error fetching exchange rates: Response content could not be deserialized."));
    }
        
    [Test]
    public void NullHttpClient_ThrowsArgumentNullException()
    {
        // Act
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new ExchangeRateService(null, _configurationMock.Object, _fakeTimeProvider, _loggerMock.Object));

        // Assert
        Assert.That(exception.ParamName, Is.EqualTo("httpClient"));
    }
        
    [Test]
    public void NullConfiguration_ThrowsArgumentNullException()
    {
        // Act
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new ExchangeRateService(_httpClient, null, _fakeTimeProvider, _loggerMock.Object));

        // Assert
        Assert.That(exception.ParamName, Is.EqualTo("configuration"));
    }
}