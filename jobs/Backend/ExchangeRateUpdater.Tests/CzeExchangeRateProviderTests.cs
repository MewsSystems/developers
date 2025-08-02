using System.Net;
using System.Xml.Serialization;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Models.Cache;
using ExchangeRateUpdater.Models.Countries.CZE;
using ExchangeRateUpdater.Services.Countries.CZE;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests;

public class CzeExchangeRateProviderTests
{
    private readonly Mock<ICacheService> _cacheMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly Mock<ILogger<CzeExchangeRateProvider>> _loggerMock = new();
    private readonly Mock<IOptionsSnapshot<CzeSettings>> _optionsMock = new();
    private readonly HttpClient _httpClient;

    private readonly CzeSettings _settings = new()
    {
        BaseUrl = "http://fake-url",
        TtlInSeconds = 60,
        UpdateHourInLocalTime = "06:00:00"
    };

    private readonly List<Currency> _currencies = new()
    {
        new Currency("USD"),
        new Currency("EUR")
    };

    public CzeExchangeRateProviderTests()
    {
        _optionsMock.Setup(o => o.Value).Returns(_settings);

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(handlerMock.Object);

        _dateTimeProviderMock.Setup(d => d.UtcNow).Returns(new DateTimeOffset(2025, 8, 2, 0, 0, 0, TimeSpan.Zero));
    }

    [Fact]
    public async Task GetExchangeRates_ReturnsEmpty_WhenBaseUrlIsEmpty()
    {
        _settings.BaseUrl = null;
        var provider = CreateProvider();

        var result = await provider.GetExchangeRates(_currencies);

        Assert.Empty(result);
        _loggerMock.VerifyLog(LogLevel.Warning, "Base URL is not configured", Times.Once());
    }

    [Fact]
    public async Task GetExchangeRates_ReturnsCachedData_IfCacheValid()
    {
        var cachedResponse = new CzeExchangeRatesResponse
        {
            Table = new CzeExchangeRateTable
            {
                Rates = new List<CzeExchangeRate>
                {
                    new() { Code = "USD", Amount = 1, RateRaw = "22,0" },
                    new() { Code = "EUR", Amount = 1, RateRaw = "24.0" },
                }
            }
        };

        var cacheObject = new CacheObject<CzeExchangeRatesResponse>
        {
            Data = cachedResponse,
            DataExtractionTimeUTC = DateTimeOffset.UtcNow.AddHours(1)
        };

        _cacheMock.Setup(c => c.GetAsync<CacheObject<CzeExchangeRatesResponse>>(It.IsAny<string>()))
            .ReturnsAsync(cacheObject);

        var provider = CreateProvider();

        var result = await provider.GetExchangeRates(_currencies);

        Assert.NotEmpty(result);
        Assert.Contains(result, r => r.TargetCurrency.Code == "USD" && r.Value == 22);
        Assert.Contains(result, r => r.TargetCurrency.Code == "EUR" && r.Value == 24);
    }

    [Fact]
    public async Task GetExchangeRates_FetchesFromRemoteAndCaches_WhenNoValidCache()
    {
        var response = new CzeExchangeRatesResponse
        {
            Table = new CzeExchangeRateTable
            {
                Rates = new List<CzeExchangeRate>
                {
                    new() { Code = "USD", Amount = 2, RateRaw = "44,0" },
                    new() { Code = "EUR", Amount = 1, RateRaw = "24.0" },
                }
            }
        };

        var memoryStream = SerializeToXmlStream(response);

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<System.Threading.CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(memoryStream)
            });

        _cacheMock.Setup(c => c.GetAsync<CacheObject<CzeExchangeRatesResponse>>(It.IsAny<string>()))
            .ReturnsAsync((CacheObject<CzeExchangeRatesResponse>)null);

        _cacheMock.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<CacheObject<CzeExchangeRatesResponse>>(), It.IsAny<TimeSpan>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var provider = CreateProvider(handlerMock.Object);

        var result = await provider.GetExchangeRates(_currencies);

        Assert.NotEmpty(result);
        Assert.Contains(result, r => r.TargetCurrency.Code == "USD" && r.Value == 22);
        Assert.Contains(result, r => r.TargetCurrency.Code == "EUR" && r.Value == 24);

        _cacheMock.Verify();
    }

    [Fact]
    public async Task GetExchangeRates_ReturnsEmpty_WhenRemoteFetchFails()
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<System.Threading.CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        _cacheMock.Setup(c => c.GetAsync<CacheObject<CzeExchangeRatesResponse>>(It.IsAny<string>()))
            .ReturnsAsync((CacheObject<CzeExchangeRatesResponse>?)null);

        var provider = CreateProvider(handlerMock.Object);

        var result = await provider.GetExchangeRates(_currencies);

        Assert.Empty(result);
    }

    [Fact]
    public void GetUpdateHourInUTC_ReturnsCorrectUtcDateTime()
    {
        var fixedNow = new DateTimeOffset(2024, 1, 1, 10, 0, 0, TimeSpan.Zero);
        _dateTimeProviderMock.Setup(d => d.UtcNow).Returns(fixedNow);

        var provider = CreateProvider();

        var result = provider.GetType()
            .GetMethod("GetUpdateHourInUTC", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(provider, null);

        Assert.IsType<DateTimeOffset>(result);

        var utcResult = (DateTimeOffset)result;

        var czechZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        var expectedLocal = new DateTime(fixedNow.Year, fixedNow.Month, fixedNow.Day, 6, 0, 0);
        var expectedUtc = new DateTimeOffset(expectedLocal, czechZone.GetUtcOffset(expectedLocal)).ToUniversalTime();

        Assert.Equal(expectedUtc, utcResult);
    }

    private static MemoryStream SerializeToXmlStream(CzeExchangeRatesResponse response)
    {
        var serializer = new XmlSerializer(typeof(CzeExchangeRatesResponse));
        var ms = new MemoryStream();
        using (var writer = new StreamWriter(ms, new System.Text.UTF8Encoding(true), 1024, leaveOpen: true))
        {
            serializer.Serialize(writer, response);
        }
        ms.Position = 0;
        return ms;
    }

    private CzeExchangeRateProvider CreateProvider(HttpMessageHandler? handler = null)
    {
        var client = handler == null ? _httpClient : new HttpClient(handler);

        return new CzeExchangeRateProvider(
            client,
            _optionsMock.Object,
            _cacheMock.Object,
            _dateTimeProviderMock.Object,
            _loggerMock.Object);
    }
}