using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Extensions;
using ExchangeRateUpdater.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Tests.Core;

public class ExchangeRateServiceTests
{
    private readonly Mock<IExchangeRateProvider> _mockProvider;
    private readonly Mock<IExchangeRateCache> _mockCache;
    private readonly Mock<ILogger<ExchangeRateService>> _mockLogger;
    private readonly Mock<IOptions<ExchangeRateOptions>> _mockOptions;
    private readonly ExchangeRateService _service;

    public ExchangeRateServiceTests()
    {
        _mockProvider = new Mock<IExchangeRateProvider>();
        _mockCache = new Mock<IExchangeRateCache>();
        _mockLogger = new Mock<ILogger<ExchangeRateService>>();
        _mockOptions = new Mock<IOptions<ExchangeRateOptions>>();

        var options = new ExchangeRateOptions
        {
            EnableCaching = true,
            DefaultCacheExpiry = TimeSpan.FromHours(1)
        };

        _mockOptions.Setup(x => x.Value).Returns(options);

        _service = new ExchangeRateService(_mockProvider.Object, _mockCache.Object, _mockLogger.Object, _mockOptions.Object);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithCachedRates_ShouldReturnCachedRates()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var cachedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25.0m, DateTime.Today),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 27.0m, DateTime.Today)
        };

        _mockCache.Setup(x => x.GetCachedRates(It.IsAny<IEnumerable<Currency>>(), It.IsAny<Maybe<DateTime>>()))
                  .ReturnsAsync(((IReadOnlyList<ExchangeRate>)cachedRates).AsMaybe());

        // Act
        var result = await _service.GetExchangeRates(currencies, DateTime.Today.AsMaybe());

        // Assert
        Assert.Equal(2, result.Count());
        _mockProvider.Verify(x => x.GetExchangeRatesForDate(It.IsAny<Maybe<DateTime>>()), Times.Never);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithoutCachedRates_ShouldFetchFromProvider()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };
        var providerRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25.0m, DateTime.Today)
        };

        _mockCache.Setup(x => x.GetCachedRates(It.IsAny<IReadOnlyList<Currency>>(), It.IsAny<Maybe<DateTime>>()))
                  .ReturnsAsync(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);

        _mockProvider.Setup(x => x.GetExchangeRatesForDate(It.IsAny<Maybe<DateTime>>()))
                     .ReturnsAsync(((IReadOnlyCollection<ExchangeRate>)providerRates).AsMaybe());

        // Act
        var result = await _service.GetExchangeRates(currencies, DateTime.Today.AsMaybe());

        // Assert
        Assert.Single(result);
        Assert.Equal("USD", result.First().SourceCurrency.Code);
        _mockCache.Verify(x => x.CacheRates(It.IsAny<IReadOnlyList<ExchangeRate>>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithNullCurrencies_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _service.GetExchangeRates(null!, DateTime.Today.AsMaybe()));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithEmptyCurrencies_ShouldReturnEmpty()
    {
        // Arrange
        var emptyCurrencies = Array.Empty<Currency>();

        // Act
        var result = await _service.GetExchangeRates(emptyCurrencies, DateTime.Today.AsMaybe());

        // Assert
        Assert.Empty(result);
    }
}
