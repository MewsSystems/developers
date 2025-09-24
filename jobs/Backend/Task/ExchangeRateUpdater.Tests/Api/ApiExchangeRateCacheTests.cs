using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Api.Services;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Extensions;
using FluentAssertions;
using NSubstitute;

namespace ExchangeRateUpdater.Tests.Api;

public class ApiExchangeRateCacheTests
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ApiExchangeRateCache> _logger;
    private readonly ApiExchangeRateCache _cache;
    private readonly DateTime TARGET_DATE = new(2025, 9, 26);

    public ApiExchangeRateCacheTests()
    {
        _memoryCache = Substitute.For<IMemoryCache>();
        _logger = Substitute.For<ILogger<ApiExchangeRateCache>>();
        _cache = new ApiExchangeRateCache(_memoryCache, _logger);
    }

    [Fact]
    public async Task GetCachedRates_WithNullCurrencies_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _cache.GetCachedRates(null!, DateTime.Today.AsMaybe()));
        
        exception.ParamName.Should().Be("currencies");
    }

    [Fact]
    public async Task GetCachedRates_WithEmptyCurrencies_ShouldReturnNothing()
    {
        // Arrange
        var emptyCurrencies = Array.Empty<Currency>();

        // Act
        var result = await _cache.GetCachedRates(emptyCurrencies, DateTime.Today.AsMaybe());

        // Assert
        result.Should().Be(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);
    }

    [Fact]
    public async Task GetCachedRates_WithCacheHit_ShouldReturnFilteredRates()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var allCachedRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.0m, TARGET_DATE),
            new(new Currency("EUR"), new Currency("CZK"), 27.0m, TARGET_DATE),
            new(new Currency("GBP"), new Currency("CZK"), 30.0m, TARGET_DATE)
        };

        _memoryCache.TryGetValue(TARGET_DATE, out Arg.Any<object>())
            .Returns(x =>
            {
                x[1] = allCachedRates;
                return true;
            });

        // Act
        var result = await _cache.GetCachedRates(currencies, TARGET_DATE.AsMaybe());

        // Assert
        result.Should().NotBe(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);
        result.HasValue.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().Contain(r => r.SourceCurrency.Code == "USD");
        result.Value.Should().Contain(r => r.SourceCurrency.Code == "EUR");
        result.Value.Should().NotContain(r => r.SourceCurrency.Code == "GBP");
    }

    [Fact]
    public async Task GetCachedRates_WithCacheMiss_ShouldReturnNothing()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };

        _memoryCache.TryGetValue(TARGET_DATE, out Arg.Any<object>())
            .Returns(false);

        // Act
        var result = await _cache.GetCachedRates(currencies, TARGET_DATE.AsMaybe());

        // Assert
        result.Should().Be(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);
    }

    [Fact]
    public async Task GetCachedRates_WithCacheHitButNoMatchingCurrencies_ShouldReturnNothing()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };
        var allCachedRates = new List<ExchangeRate>
        {
            new(new Currency("EUR"), new Currency("CZK"), 27.0m, TARGET_DATE),
            new(new Currency("GBP"), new Currency("CZK"), 30.0m, TARGET_DATE)
        };

        _memoryCache.TryGetValue(TARGET_DATE, out Arg.Any<object>())
            .Returns(x =>
            {
                x[1] = allCachedRates;
                return true;
            });

        // Act
        var result = await _cache.GetCachedRates(currencies, TARGET_DATE.AsMaybe());

        // Assert
        result.Should().Be(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);
    }

    [Fact]
    public async Task CacheRates_WithNullRates_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _cache.CacheRates(null!, TimeSpan.FromHours(1)));
        
        exception.ParamName.Should().Be("rates");
    }

    [Fact]
    public async Task CacheRates_WithDifferentDates_ShouldCacheWithFirstRateDate()
    {
        // Arrange
        var yesterday = TARGET_DATE.AddDays(-1);
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.0m, yesterday),
            new(new Currency("EUR"), new Currency("CZK"), 27.0m, TARGET_DATE)
        };

        // Act
        await _cache.CacheRates(rates, TimeSpan.FromHours(1));

        // Assert
        _memoryCache.Received(1).Set(
            yesterday,
            rates);
    }
}
