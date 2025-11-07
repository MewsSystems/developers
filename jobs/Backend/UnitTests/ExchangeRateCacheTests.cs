using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.UnitTests;

public class ExchangeRateCacheTests
{
    private readonly IMemoryCache _memoryCache;
    private readonly Mock<ILogger<ExchangeRateCache>> _loggerMock;
    private readonly IOptions<CnbExchangeRateConfiguration> _configuration;
    private readonly ExchangeRateCache _cache;

    public ExchangeRateCacheTests()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _loggerMock = new Mock<ILogger<ExchangeRateCache>>();
        _configuration = Options.Create(new CnbExchangeRateConfiguration
        {
            CacheDurationMinutes = 60
        });
        _cache = new ExchangeRateCache(_memoryCache, _loggerMock.Object, _configuration);
    }

    [Fact]
    public void GetCachedRates_WhenEmpty_ReturnsNull()
    {
        // Arrange
        var currencyCodes = new[] { "USD", "EUR" };

        // Act
        var result = _cache.GetCachedRates(currencyCodes);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void SetCachedRates_ThenGet_ReturnsRates()
    {
        // Arrange
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.95m),
            new(new Currency("EUR"), new Currency("CZK"), 24.375m)
        };
        var currencyCodes = new[] { "USD", "EUR" };

        // Act
        _cache.SetCachedRates(rates);
        var result = _cache.GetCachedRates(currencyCodes);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.SourceCurrency.Code == "USD");
        result.Should().Contain(r => r.SourceCurrency.Code == "EUR");
    }

    [Fact]
    public void GetCachedRates_OrderIndependent()
    {
        // Arrange
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.95m),
            new(new Currency("EUR"), new Currency("CZK"), 24.375m)
        };

        _cache.SetCachedRates(rates);

        // Act - Try different order
        var result1 = _cache.GetCachedRates(new[] { "USD", "EUR" });
        var result2 = _cache.GetCachedRates(new[] { "EUR", "USD" });

        // Assert - Both should return cached data
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().HaveCount(2);
        result2.Should().HaveCount(2);
    }

    [Fact]
    public void SetCachedRates_WithEmptyList_DoesNotCache()
    {
        // Arrange
        var emptyRates = new List<ExchangeRate>();

        // Act
        _cache.SetCachedRates(emptyRates);
        var result = _cache.GetCachedRates(new[] { "USD" });

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void SetCachedRates_WithNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        _cache.Invoking(c => c.SetCachedRates(null!))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetCachedRates_DifferentCurrencies_ReturnsDifferentCache()
    {
        // Arrange
        var usdRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.95m)
        };
        var eurRates = new List<ExchangeRate>
        {
            new(new Currency("EUR"), new Currency("CZK"), 24.375m)
        };

        _cache.SetCachedRates(usdRates);

        // Act
        var usdResult = _cache.GetCachedRates(new[] { "USD" });
        var eurResult = _cache.GetCachedRates(new[] { "EUR" });

        // Assert
        usdResult.Should().NotBeNull().And.HaveCount(1);
        eurResult.Should().BeNull(); // EUR not cached
    }
}
