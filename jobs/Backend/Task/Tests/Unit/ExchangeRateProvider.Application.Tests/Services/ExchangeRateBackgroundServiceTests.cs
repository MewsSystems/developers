namespace ExchangeRateProvider.Application.Tests.Services;

using Interfaces;
using ExchangeRateProvider.Application.Services;
using Domain.Entities;
using Domain.Options;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

[TestFixture]
public class ExchangeRateBackgroundServiceTests
{
    [SetUp]
    public void Setup()
    {
        _exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
        _loggerMock = new Mock<ILogger<ExchangeRateBackgroundService>>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());

        _options = Options.Create(new CnbApiOptions
        {
            CacheDurationHours = 1,
            UpdateHour = DateTime.UtcNow.Hour,
            UpdateMinute = DateTime.UtcNow.Minute + 1,
            BaseUrl = "https://api.example.com"
        });

        _service = new ExchangeRateBackgroundService(_memoryCache, _exchangeRateProviderMock.Object, _loggerMock.Object,
            _options);
    }

    [TearDown]
    public void TearDown()
    {
        _memoryCache.Dispose();
        _service.Dispose();
    }

    private Mock<IExchangeRateProvider> _exchangeRateProviderMock;
    private Mock<ILogger<ExchangeRateBackgroundService>> _loggerMock;
    private IOptions<CnbApiOptions> _options;
    private IMemoryCache _memoryCache;
    private ExchangeRateBackgroundService _service;

    [Test]
    public async Task ExecuteAsync_ShouldUpdateExchangeRates_WhenCalled()
    {
        // Arrange
        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.5),
            new(new Currency("EUR"), new Currency("CZK"), 25.3)
        };

        _exchangeRateProviderMock
            .Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(exchangeRates);

        // Act
        await _service.StartAsync(CancellationToken.None);
        await Task.Delay(500);

        // Assert
        _memoryCache.TryGetValue("ExchangeRates", out List<ExchangeRate>? cachedRates);
        cachedRates.Should().NotBeNull().And.HaveCount(2);
        cachedRates.Should().Contain(r => r.SourceCurrency.Code == "USD" && r.Value == 22.5);
        cachedRates.Should().Contain(r => r.SourceCurrency.Code == "EUR" && r.Value == 25.3);
    }

    [Test]
    public async Task ExecuteAsync_ShouldNotUpdateCache_WhenExchangeRatesAreEmpty()
    {
        // Arrange
        _exchangeRateProviderMock
            .Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(new List<ExchangeRate>());

        // Act
        await _service.StartAsync(CancellationToken.None);
        await Task.Delay(500);

        // Assert
        _memoryCache.TryGetValue("ExchangeRates", out List<ExchangeRate>? cachedRates);
        cachedRates.Should().BeNull();
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Contains("Failed to retrieve exchange rates. The cache was not updated.")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_ShouldLogError_WhenApiFails()
    {
        // Arrange
        _exchangeRateProviderMock
            .Setup(x => x.GetExchangeRatesAsync())
            .ThrowsAsync(new Exception("API error"));

        // Act
        await _service.StartAsync(CancellationToken.None);
        await Task.Delay(500);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Error while updating exchange rates.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);
    }

    [Test]
    public async Task ScheduleNextUpdate_ShouldSetTimerToNextDay_WhenCurrentTimeHasPassedUpdateTime()
    {
        // Arrange
        var testOptions = Options.Create(new CnbApiOptions
        {
            CacheDurationHours = 1,
            UpdateHour = DateTime.UtcNow.Hour - 1,
            UpdateMinute = DateTime.UtcNow.Minute,
            BaseUrl = "https://api.example.com"
        });

        var service = new ExchangeRateBackgroundService(_memoryCache, _exchangeRateProviderMock.Object,
            _loggerMock.Object, testOptions);

        // Act
        await service.StartAsync(CancellationToken.None);
        await Task.Delay(500);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>(
                    (o, t) => o.ToString()!.Contains("Next exchange rate update scheduled for tomorrow")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);
    }

    [Test]
    public async Task StopAsync_ShouldCancelScheduledUpdates()
    {
        // Arrange
        var service = new ExchangeRateBackgroundService(_memoryCache, _exchangeRateProviderMock.Object,
            _loggerMock.Object, _options);

        // Act
        await service.StartAsync(CancellationToken.None);

        await Task.Delay(500);

        await service.StopAsync(CancellationToken.None);

        // Assert
        _exchangeRateProviderMock.Verify(x => x.GetExchangeRatesAsync(), Times.AtMostOnce(),
            "Service should not fetch rates after stopping.");
    }
}
