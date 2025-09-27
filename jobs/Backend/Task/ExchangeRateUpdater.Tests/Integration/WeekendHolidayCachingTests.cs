using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Api.Services;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Extensions;
using FluentAssertions;
using NSubstitute;

namespace ExchangeRateUpdater.Tests.Integration;

public class WeekendHolidayCachingTests
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ApiExchangeRateCache> _logger;
    private readonly ApiExchangeRateCache _sut;

    public WeekendHolidayCachingTests()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _logger = Substitute.For<ILogger<ApiExchangeRateCache>>();
        _sut = new ApiExchangeRateCache(_memoryCache, _logger);
    }

    [Fact]
    public async Task GetCachedRates_OnWeekend_ShouldReturnPreviousBusinessDayRates()
    {
        // Arrange
        var friday = new DateTime(2024, 1, 5); // Friday
        var saturday = new DateTime(2024, 1, 6); // Saturday
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.0m, friday),
            new(new Currency("EUR"), new Currency("CZK"), 27.0m, friday)
        };

        await _sut.CacheRates(rates, TimeSpan.FromHours(1));

        // Act
        var result = await _sut.GetCachedRates(
            [new Currency("USD"), new Currency("EUR")], 
            saturday.AsMaybe());

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
        var monday = new DateTime(2024, 1, 1);
        var tuesday = new DateTime(2024, 1, 2);
        var mondayRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.0m, monday)
        };
        var tuesdayRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 26.0m, tuesday)
        };

        // Act
        await _sut.CacheRates(mondayRates, TimeSpan.FromHours(1));
        await _sut.CacheRates(tuesdayRates, TimeSpan.FromHours(1));

        // Assert
        var cachedMondayRates = _memoryCache.Get<IEnumerable<ExchangeRate>>(monday);
        var cachedTuesdayRates = _memoryCache.Get<IEnumerable<ExchangeRate>>(tuesday);

        cachedMondayRates.Should().NotBeNull();
        cachedTuesdayRates.Should().NotBeNull();
        cachedMondayRates.Should().HaveCount(1);
        cachedTuesdayRates.Should().HaveCount(1);
        cachedMondayRates.First().Value.Should().Be(25.0m);
        cachedTuesdayRates.First().Value.Should().Be(26.0m);
    }

    [Fact]
    public async Task GetCachedRates_WithPartialCurrencyMatch_ShouldReturnOnlyMatchingCurrencies()
    {
        // Arrange
        var businessDay = new DateTime(2024, 1, 2); // Tuesday
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.0m, businessDay),
            new(new Currency("EUR"), new Currency("CZK"), 27.0m, businessDay),
            new(new Currency("GBP"), new Currency("CZK"), 30.0m, businessDay)
        };

        await _sut.CacheRates(rates, TimeSpan.FromHours(1));

        // Act
        var result = await _sut.GetCachedRates(
            [new Currency("USD"), new Currency("GBP")], 
            businessDay.AsMaybe());

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
        var businessDay = new DateTime(2024, 1, 2); // Tuesday
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.0m, businessDay)
        };

        await _sut.CacheRates(rates, TimeSpan.FromHours(1));

        // Act
        var result = await _sut.GetCachedRates(
            [new Currency("usd")],
            businessDay.AsMaybe());

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
