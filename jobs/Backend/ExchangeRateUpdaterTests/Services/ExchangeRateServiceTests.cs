using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterTests.Services;

public class ExchangeRateServiceTests
{
    private Mock<ILogger<ExchangeRateService>> _loggerMock;
    private ExchangeRateService _exchangeRateService;
    private Mock<TimeProvider> _timeProviderMock;
    private Mock<HttpMessageHandler> _handlerMock;
    private HttpClient _httpClient;
    private MemoryCache _cache;
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

    [SetUp]
    public void Setup()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("http://testapi.com")
        };
        _loggerMock = new Mock<ILogger<ExchangeRateService>>();
        _timeProviderMock = new Mock<TimeProvider>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();

        _httpClientFactoryMock.Setup(factory => factory.CreateClient(nameof(ExchangeRateService)))
            .Returns(_httpClient);

        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(new Random().Next(0, 100)),
                (outcome, timeSpan, retryCount, context) =>
                {
                    _loggerMock.Object.LogWarning($"Retry {retryCount} after {timeSpan.TotalSeconds}s due to {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                });
        _exchangeRateService = new ExchangeRateService(_httpClientFactoryMock.Object,
              _loggerMock.Object, _timeProviderMock.Object, _cache);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
        _cache.Dispose();
    }

    [Test]
    public async Task GetExchangeRateListAsync_ShouldReturnExchangeRateListDto_WhenApiCallIsSuccessful()
    {
        // Arrange
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"rates\": [{\"validFor\": \"2025-02-20\", \"order\": 1, \"currency\": \"Currency\", \"country\": \"Country\", \"amount\": 1, \"currencyCode\": \"CUR\", \"rate\": 15.852}]}")
            });

        var utcNow = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero);
        _timeProviderMock.Setup(tp => tp.GetUtcNow()).Returns(utcNow);

        // Act
        var result = await _exchangeRateService.GetExchangeRateListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Rates.Should().NotBeNull();
        result.Rates.Should().HaveCount(1);
        result.Rates.Should().Contain(rate => rate.ValidFor == "2025-02-20" && rate.Order == 1 && rate.Currency == "Currency" && rate.Country == "Country" && rate.Amount == 1 && rate.CurrencyCode == "CUR" && rate.Rate == 15.852m);

        // Verify cache was used
        var cacheKey = $"ExchangeRates_{utcNow:yyyy-MM-dd}";
        _cache.TryGetValue(cacheKey, out ExchangeRateListDto cachedRates);
        cachedRates.Should().BeEquivalentTo(result);
    }

    [Test]
    public async Task GetExchangeRateListAsync_ShouldLogErrors_WhenApiCallFails()
    {
        // Arrange
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
            });

        var utcNow = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero);
        _timeProviderMock.Setup(tp => tp.GetUtcNow()).Returns(utcNow);

        // Act & Assert
        await FluentActions.Awaiting(() => _exchangeRateService.GetExchangeRateListAsync())
            .Should().ThrowAsync<HttpRequestException>();

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.AtLeastOnce);
    }

    [Test]
    public async Task GetExchangeRateListAsync_ShouldReturnCachedData_WhenAvailable()
    {
        // Arrange
        var utcNow = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero);
        _timeProviderMock.Setup(tp => tp.GetUtcNow()).Returns(utcNow);

        var cachedRates = new ExchangeRateListDto
        {
            Rates = new List<ExchangeRateDto>
            {
                new ExchangeRateDto { ValidFor = "2025-02-20", Order = 1, Currency = "Currency", Country = "Country", Amount = 1, CurrencyCode = "CUR", Rate = 15.852m }
            }
        };

        var cacheKey = $"ExchangeRates_{utcNow:yyyy-MM-dd}";
        _cache.Set(cacheKey, cachedRates);

        // Act
        var result = await _exchangeRateService.GetExchangeRateListAsync();

        // Assert
        result.Should().BeEquivalentTo(cachedRates);
        _handlerMock.Protected().Verify("SendAsync", Times.Never(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public async Task GetExchangeRateListAsync_ShouldFetchAndCacheData_WhenCacheMiss()
    {
        // Arrange
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"rates\": [{\"validFor\": \"2025-02-20\", \"order\": 1, \"currency\": \"Currency\", \"country\": \"Country\", \"amount\": 1, \"currencyCode\": \"CUR\", \"rate\": 15.852}]}")
            });

        var utcNow = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero);
        _timeProviderMock.Setup(tp => tp.GetUtcNow()).Returns(utcNow);

        // Act
        var result = await _exchangeRateService.GetExchangeRateListAsync();

        // Assert
        result.Should().NotBeNull();
        var cacheKey = $"ExchangeRates_{utcNow:yyyy-MM-dd}";
        _cache.TryGetValue(cacheKey, out ExchangeRateListDto cachedRates);
        cachedRates.Should().BeEquivalentTo(result);
    }

    [Test]
    public async Task GetExchangeRateListAsync_ShouldCacheData_WhenApiCallIsSuccessful()
    {
        // Arrange
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"rates\": [{\"validFor\": \"2025-02-20\", \"order\": 1, \"currency\": \"Currency\", \"country\": \"Country\", \"amount\": 1, \"currencyCode\": \"CUR\", \"rate\": 15.852}]}")
            });

        var utcNow = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero);
        _timeProviderMock.Setup(tp => tp.GetUtcNow()).Returns(utcNow);

        // Act
        await _exchangeRateService.GetExchangeRateListAsync();

        // Assert
        var cacheKey = $"ExchangeRates_{utcNow:yyyy-MM-dd}";
        _cache.TryGetValue(cacheKey, out ExchangeRateListDto cachedRates);
        cachedRates.Should().NotBeNull();
    }

    [Test]
    public async Task GetExchangeRateListAsync_ShouldRetry_WhenApiFails()
    {
        var retryCount = 0;
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                retryCount++;
                return retryCount < 3
                    ? new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError }
                    : new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent("{\"rates\": [{\"validFor\": \"2025-02-20\", \"order\": 1, \"currency\": \"Currency\", \"country\": \"Country\", \"amount\": 1, \"currencyCode\": \"CUR\", \"rate\": 15.852}]}")
                    };
            });

        var utcNow = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero);
        _timeProviderMock.Setup(tp => tp.GetUtcNow()).Returns(utcNow);

        // Act
        var result = await _exchangeRateService.GetExchangeRateListAsync();

        // Assert
        result.Should().NotBeNull();
        retryCount.Should().Be(3);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Exactly(2)
        );
    }
}