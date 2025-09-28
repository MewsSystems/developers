using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Infrastructure.Caching;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Extensions;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Tests.Api;

public class ApiExchangeRateCacheTests
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ApiExchangeRateCache> _logger;
    private readonly IOptions<CacheSettings> _cacheSettings;
    private readonly ApiExchangeRateCache _sut;
    private readonly DateOnly _testDate = new(2025, 9, 26);

    public ApiExchangeRateCacheTests()
    {
        _memoryCache = Substitute.For<IMemoryCache>();
        _logger = Substitute.For<ILogger<ApiExchangeRateCache>>();
        _cacheSettings = Substitute.For<IOptions<CacheSettings>>();
        _cacheSettings.Value.Returns(new CacheSettings
        {
            DefaultCacheExpiry = TimeSpan.FromHours(1)
        });

        _sut = new ApiExchangeRateCache(_memoryCache, _logger, _cacheSettings);
    }

    [Fact]
    public async Task GetCachedRates_WithNullCurrencies_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _sut.GetCachedRates(null!, DateHelper.Today));

        exception.ParamName.Should().Be("currencies");
    }

    [Fact]
    public async Task GetCachedRates_WithEmptyCurrencies_ShouldReturnNothing()
    {
        // Arrange
        var emptyCurrencies = Array.Empty<Currency>();

        // Act
        var result = await _sut.GetCachedRates(emptyCurrencies, DateHelper.Today);

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
            new(new Currency("USD"), new Currency("CZK"), 25.0m, _testDate),
            new(new Currency("EUR"), new Currency("CZK"), 27.0m, _testDate),
            new(new Currency("GBP"), new Currency("CZK"), 30.0m, _testDate)
        };

        var expectedCacheKey = $"ExchangeRates_{_testDate:yyyy-MM-dd}";
        _memoryCache.TryGetValue(expectedCacheKey, out object? _)
            .Returns(x =>
            {
                x[1] = allCachedRates;
                return true;
            });

        // Act
        var result = await _sut.GetCachedRates(currencies, _testDate);

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

        var expectedCacheKey = $"ExchangeRates_{_testDate:yyyy-MM-dd}";
        _memoryCache.TryGetValue(expectedCacheKey, out object? _)
            .Returns(false);

        // Act
        var result = await _sut.GetCachedRates(currencies, _testDate);

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
            new(new Currency("EUR"), new Currency("CZK"), 27.0m, _testDate),
            new(new Currency("GBP"), new Currency("CZK"), 30.0m, _testDate)
        };

        var expectedCacheKey = $"ExchangeRates_{_testDate:yyyy-MM-dd}";
        _memoryCache.TryGetValue(expectedCacheKey, out object? _)
            .Returns(x =>
            {
                x[1] = allCachedRates;
                return true;
            });

        // Act
        var result = await _sut.GetCachedRates(currencies, _testDate);

        // Assert
        result.Should().Be(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);
    }

    [Fact]
    public async Task CacheRates_WithNullRates_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _sut.CacheRates(null!));

        exception.ParamName.Should().Be("rates");
    }
}
