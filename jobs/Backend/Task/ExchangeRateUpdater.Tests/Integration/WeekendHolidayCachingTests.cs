using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Infrastructure.Caching;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Common;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Tests.Integration;

public class WeekendHolidayCachingTests
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ApiExchangeRateCache> _logger;
    private readonly ApiExchangeRateCache _sut;
    private readonly IOptions<CacheSettings> _cacheSettings;

    public WeekendHolidayCachingTests()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _logger = Substitute.For<ILogger<ApiExchangeRateCache>>();
        _cacheSettings = Substitute.For<IOptions<CacheSettings>>();
        _cacheSettings.Value.Returns(new CacheSettings
        {
            DefaultCacheExpiry = TimeSpan.FromHours(1)
        });

        _sut = new ApiExchangeRateCache(_memoryCache, _logger, _cacheSettings);
    }

    [Fact]
    public async Task GetCachedRates_OnWeekend_ShouldReturnPreviousBusinessDayRates()
    {
        // Arrange
        var friday = new DateOnly(2024, 1, 5); // Friday
        var saturday = new DateOnly(2024, 1, 6); // Saturday
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.0m, friday),
            new(new Currency("EUR"), new Currency("CZK"), 27.0m, friday)
        };

        await _sut.CacheRates(rates);

        // Act
        var result = await _sut.GetCachedRates(
            [new Currency("USD"), new Currency("EUR")],
            saturday);

        // Assert
        result.Should().NotBe(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);
        result.HasValue.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().AllSatisfy(rate => rate.Date.Should().Be(friday));
    }

    [Fact]
    public async Task CacheRates_WithDifferentDates_ShouldCacheSeparately()
    {
        // Arrange
        var tuesday = new DateOnly(2024, 1, 2);
        var wednesday = new DateOnly(2024, 1, 3);
        var tuesdayRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.0m, tuesday)
        };
        var wednesdayRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 26.0m, wednesday)
        };

        // Act
        await _sut.CacheRates(tuesdayRates);
        await _sut.CacheRates(wednesdayRates);

        // Assert
        var tuesdayCachedRates = _memoryCache.Get<IEnumerable<ExchangeRate>>($"ExchangeRates_{tuesday:yyyy-MM-dd}");
        var wednesdayCachedRates = _memoryCache.Get<IEnumerable<ExchangeRate>>($"ExchangeRates_{wednesday:yyyy-MM-dd}");

        tuesdayCachedRates.Should().NotBeNull();
        wednesdayCachedRates.Should().NotBeNull();
        tuesdayCachedRates.Should().HaveCount(1);
        wednesdayCachedRates.Should().HaveCount(1);
        tuesdayCachedRates!.First().Value.Should().Be(25.0m);
        wednesdayCachedRates!.First().Value.Should().Be(26.0m);
    }

    [Fact]
    public async Task GetCachedRates_WithPartialCurrencyMatch_ShouldReturnOnlyMatchingCurrencies()
    {
        // Arrange
        var businessDay = new DateOnly(2024, 1, 2); // Tuesday
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.0m, businessDay),
            new(new Currency("EUR"), new Currency("CZK"), 27.0m, businessDay),
            new(new Currency("GBP"), new Currency("CZK"), 30.0m, businessDay)
        };

        await _sut.CacheRates(rates);

        // Act
        var result = await _sut.GetCachedRates(
            [new Currency("USD"), new Currency("GBP")],
            businessDay);

        // Assert
        result.Should().NotBe(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);
        result.HasValue.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().Contain(r => r.SourceCurrency.Code == "USD");
        result.Value.Should().Contain(r => r.SourceCurrency.Code == "GBP");
        result.Value.Should().NotContain(r => r.SourceCurrency.Code == "EUR");
    }

    [Fact]
    public async Task GetCachedRates_WithCaseInsensitiveCurrencyMatch_ShouldReturnRates()
    {
        // Arrange
        var businessDay = new DateOnly(2024, 1, 2); // Tuesday
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.0m, businessDay)
        };

        await _sut.CacheRates(rates);

        // Act
        var result = await _sut.GetCachedRates(
            [new Currency("usd")],
            businessDay);

        // Assert
        result.Should().NotBe(Maybe<IReadOnlyList<ExchangeRate>>.Nothing);
        result.HasValue.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().SourceCurrency.Code.Should().Be("USD");
    }

    private void Dispose()
    {
        _memoryCache?.Dispose();
    }
}
