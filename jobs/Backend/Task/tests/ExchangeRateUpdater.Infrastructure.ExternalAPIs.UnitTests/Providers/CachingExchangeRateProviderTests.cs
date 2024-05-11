using AutoFixture;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Clients;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Models;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Options;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Providers;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.UnitTests.Providers;

public class CachingExchangeRateProviderTests
{
    private readonly CachingExchangeRateProvider _sut;
    private readonly IExchangeRateClient _client = Substitute.For<IExchangeRateClient>();
    private readonly IMemoryCache _memoryCache;
    public CachingExchangeRateProviderTests()
    {
        var memoryCacheOptions = Microsoft.Extensions.Options.Options.Create(new MemoryCacheOptions());
        _memoryCache = new MemoryCache(memoryCacheOptions);
        
        var options = new ExchangeRateProviderCachingOptions()
        {
            Enabled = true,
            MaxCacheDuration = TimeSpan.FromMinutes(5),
            SlidingCacheDuration = TimeSpan.FromMinutes(5)
        };
        var cacheOptions = Microsoft.Extensions.Options.Options.Create(options);
        
        var timeProvider = Substitute.For<TimeProvider>();
        timeProvider
            .GetUtcNow()
            .Returns(new DateTimeOffset(new DateTime(2024, 5, 9)));
        
        _sut = new CachingExchangeRateProvider(_client, _memoryCache, cacheOptions, timeProvider);
    }

    [Fact]
    public async Task GetExchangeRates_Returns_Expected_Rates()
    {
        var fixture = new Fixture();
        CnbRate[] cnbRates = {
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "USD").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "GBP").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "JPY").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "CZH").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "AUD").Create(),
        };
        Currency[] filterCurrencies =
        {
            new("USD"),
            new("CZH"),
            new("Aud"), // Should work case-insensitively
        };

        _client.GetRates(Arg.Any<CancellationToken>()).Returns(cnbRates);

        var result = await _sut.GetExchangeRates(filterCurrencies, CancellationToken.None);

        result.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task GetExchangeRates_Returns_Expected_Rates_When_Date_Provided()
    {
        var fixture = new Fixture();
        CnbRate[] cnbRates =
        {
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "USD").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "GBP").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "JPY").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "CZH").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "AUD").Create(),
        };
        Currency[] filterCurrencies =
        {
            new("USD"),
            new("Aud"), // Should work case-insensitively
        };

        var date = new DateTime(2023, 4, 2);
        _client.GetRates(Arg.Is(date), Arg.Any<CancellationToken>()).Returns(cnbRates);

        var result = await _sut.GetExchangeRates(date, filterCurrencies, CancellationToken.None);
        result.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetExchangeRates_Caches_Rates_Using_ValidFor_Date()
    {
        var fixture = new Fixture();
        CnbRate[] cnbRates =
        {
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "USD").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "GBP").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "JPY").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "CZH").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "AUD").Create(),
        };
        Currency[] filterCurrencies =
        {
            new("USD"),
            new("CZH"),
            new("Aud"), // Should work case-insensitively
        };

        _client.GetRates(Arg.Any<CancellationToken>()).Returns(cnbRates);
        var expectedCacheDate = cnbRates[0].ValidFor;

        var result = await _sut.GetExchangeRates(filterCurrencies, CancellationToken.None);

        result.Should().HaveCount(3);
        _memoryCache.TryGetValue($"exchange-rates-{expectedCacheDate:yyyy-MM-dd}", out IEnumerable<CnbRate>? actualCachedRates).Should().BeTrue();
        actualCachedRates.Should().BeEquivalentTo(cnbRates);
    }
    
    [Fact]
    public async Task GetExchangeRates_Caches_Rate_Using_ValidFor_Date_Even_When_Date_Is_Provided()
    {
        var fixture = new Fixture();
        CnbRate[] cnbRates =
        {
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "USD").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "GBP").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "JPY").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "CZH").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "AUD").Create(),
        };
        Currency[] filterCurrencies =
        {
            new("USD"),
            new("Aud"), // Should work case-insensitively
        };

        var date = new DateTime(2023, 4, 2);
        var expectedCacheDate = cnbRates[0].ValidFor;
        _client.GetRates(Arg.Is(date), Arg.Any<CancellationToken>()).Returns(cnbRates);

        var result = await _sut.GetExchangeRates(date, filterCurrencies, CancellationToken.None);

        result.Should().HaveCount(2);
        _memoryCache.TryGetValue($"exchange-rates-{expectedCacheDate:yyyy-MM-dd}", out IEnumerable<CnbRate>? actualCachedRates).Should().BeTrue();
        actualCachedRates.Should().BeEquivalentTo(cnbRates);
    }
    
    [Fact]
    public async Task GetExchangeRates_Takes_Data_From_Cache_When_It_Exists()
    {
        var fixture = new Fixture();
        CnbRate[] cnbRates =
        {
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "USD").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "GBP").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "JPY").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "CZH").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "AUD").Create(),
        };
        var date = new DateTime(2023, 4, 2);
        var cacheKey = $"exchange-rates-{date:yyyy-MM-dd}";
        _memoryCache.Set(cacheKey, cnbRates);
        Currency[] filterCurrencies =
        {
            new("USD"),
            new("Aud"), // Should work case-insensitively
        };

        var result = await _sut.GetExchangeRates(date, filterCurrencies, CancellationToken.None);

        // We're using the cache, so we shouldn't attempt any client calls.
        await _client.DidNotReceive().GetRates(Arg.Any<DateTime>(), Arg.Any<CancellationToken>());
        result.Should().HaveCount(2);
        _memoryCache.TryGetValue(cacheKey, out IEnumerable<CnbRate>? actualCachedRates).Should().BeTrue();
        actualCachedRates.Should().BeEquivalentTo(cnbRates);
    }
    
    [Fact]
    public async Task GetExchangeRates_Returns_Empty_When_No_Rates_Are_Available()
    {
        CnbRate[]? cnbRates = null;
        Currency[] filterCurrencies =
        {
            new("USD"),
            new("CZH"),
            new("Aud"), // Should work case-insensitively
        };

        _client.GetRates(Arg.Any<CancellationToken>()).Returns(cnbRates);

        var result = await _sut.GetExchangeRates(filterCurrencies, CancellationToken.None);
        result.Should().BeEmpty();
    }
}