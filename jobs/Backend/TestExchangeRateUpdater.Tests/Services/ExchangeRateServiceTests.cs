using FluentAssertions.Execution;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using System.Net;

namespace TestExchangeRateUpdater.Tests.Services;

[TestFixture]
public class ExchangeRateServiceTests
{
    private MemoryCache _memoryCache;
    private Mock<ILogger<ExchangeRateService>> _loggerMock;
    private ExchangeRateService _exchangeRateService;
    private Mock<TimeProvider> _timeProviderMock;

    [SetUp]
    public void Setup()
    {
        _timeProviderMock = new Mock<TimeProvider>();
        _timeProviderMock
            .Setup(x => x.GetUtcNow())
            .Returns(new DateTime(2025, 1, 11));

        _memoryCache = new MemoryCache(new MemoryCacheOptions());

        _loggerMock = new Mock<ILogger<ExchangeRateService>>();

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"rates\": [{\"validFor\": \"2025-01-11\", \"order\": 1, \"currency\": \"Currency\", \"country\": \"Country\", \"amount\": 1, \"currencyCode\": \"CUR\", \"rate\": 10.00}]}")
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://baseaddress/")
        };

        _exchangeRateService = new ExchangeRateService(httpClient,
            _timeProviderMock.Object,
            _memoryCache,
            _loggerMock.Object);

    }

    [TearDown]
    public void TearDown()
    {
        _memoryCache.Dispose();
    }

    [Test]
    public void GetExchangeRatesAsync_ShouldReturnCorrectExchangeRatesDTO_AndSetCache()
    {
        // Arrange
        var expected = new ExchangeRatesDTO
        {
            Rates = new List<ExchangeRateDTO>
            {
                new ()
                {
                    Amount = 1,
                    CurrencyCode = "CUR",
                    Rate = 10.00M
                }
            }
        };

        // Act
        var actual = _exchangeRateService.GetExchangeRatesAsync().Result;

        // Assert
        using (new AssertionScope())
        {
            actual.Should().BeEquivalentTo(expected);

            var cachedRates = _memoryCache.Get<ExchangeRatesDTO>("ExchangeRates");
            cachedRates.Should().BeEquivalentTo(expected);
        }
    }

    [Test]
    public void GetExchangeRatesAsync_ShouldNotSetCache_WhenResponseIsSuccessfullButReturnsNoRates()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"rates\": []}")
            });
        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://baseaddress/")
        };

        _exchangeRateService = new ExchangeRateService(httpClient,
            _timeProviderMock.Object,
            _memoryCache,
            _loggerMock.Object);

        // Act
        _ = _exchangeRateService.GetExchangeRatesAsync().Result;

        // Assert
        _memoryCache.TryGetValue("ExchangeRates", out ExchangeRatesDTO result).Should().BeFalse();
    }

    [Test]
    public void GetExchangeRatesAsync_ShouldReturnCachedExchangeRatesDTO_AndLogInformation_WhenCacheIsNotEmpty()
    {
        // Arrange
        var expected = new ExchangeRatesDTO
        {
            Rates = new List<ExchangeRateDTO>
            {
                new ()
                {
                    Amount = 1,
                    CurrencyCode = "XXX",
                    Rate = 20.00M
                }
            }
        };

        _memoryCache.Set("ExchangeRates", expected);

        // Act
        var actual = _exchangeRateService.GetExchangeRatesAsync().Result;

        // Assert
        using (new AssertionScope())
        {
            actual.Should().BeEquivalentTo(expected);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Exchange rates retrieved from cache")),
                    It.IsAny<ExchangeRateServiceException>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
    }

    [Test]
    public void GetExchangeRatesAsync_ShouldThrowException_WhenResponseIsNotSuccessful()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });
        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://baseaddress/")
        };

        _exchangeRateService = new ExchangeRateService(httpClient,
            _timeProviderMock.Object,
            _memoryCache,
            _loggerMock.Object);

        // Act and Assert
        using (new AssertionScope())
        {
            var exception = Assert.ThrowsAsync<ExchangeRateServiceException>(() => _exchangeRateService.GetExchangeRatesAsync());
            exception.Message.Should().Be("Failed to get exchange rates. Status code: BadRequest");
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to get exchange rates. Status code: BadRequest")),
                    null,
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
    }
}
