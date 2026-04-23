using ExchangeRateUpdater.Worker.Services;
using ExchangeRateUpdater.Application.ExchangeRates.Dtos;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Domain.Enums;
using Microsoft.Extensions.Logging.Abstractions;
using UnitTests.Fakers;
using ExchangeRateUpdater.Application.Common.Utils;

namespace UnitTests.Services;

public class PerDayProcessorTests
{
    [Fact]
    public async Task ProcessDateAsync_Skips_When_Cache_Hit()
    {
        var date = DateTime.UtcNow.Date;

        var key = CacheKeyHelper.RatesKey(Language.EN, date);

        var cache = new FakeCacheRepository();
        await cache.SetRatesAsync(key, new Dictionary<string, ExchangeRate> { { "USD", new ExchangeRate(new Currency("USD"), new Currency("CZK"), 1m) } });

        var api = new FakeExchangeRateApiClient
        {
            OnGetExchangeRatesAsync = (d, l, ct) => Task.FromResult<IEnumerable<ExchangeRateApiDto>>(new[] { new ExchangeRateApiDto { ValidFor = date.ToString("yyyy-MM-dd"), Amount = 1, CurrencyCode = "USD", Rate = 25m } })
        };

        var p = new PerDayProcessor(api, cache, new NullLogger<PerDayProcessor>());

        await p.ProcessDateAsync(date, Language.EN, CancellationToken.None);

        // Ensure cache was not overwritten (still 1 item)
        var cached = await cache.GetRatesDictionaryAsync(key);
        cached.ShouldNotBeNull();
        cached.Count.ShouldBe(1);
    }

    [Fact]
    public async Task ProcessDateAsync_Does_Nothing_When_Api_Returns_Empty()
    {
        var date = DateTime.UtcNow.Date;
        var key = CacheKeyHelper.RatesKey(Language.EN, date);

        var cache = new FakeCacheRepository();

        var api = new FakeExchangeRateApiClient
        {
            OnGetExchangeRatesAsync = (d, l, ct) => Task.FromResult<IEnumerable<ExchangeRateApiDto>>(Array.Empty<ExchangeRateApiDto>())
        };

        var p = new PerDayProcessor(api, cache, new NullLogger<PerDayProcessor>());

        await p.ProcessDateAsync(date, Language.EN, CancellationToken.None);

        var cached = await cache.GetRatesDictionaryAsync(key);
        cached.ShouldBeNull();
    }

    [Fact]
    public async Task ProcessDateAsync_Caches_Api_Results()
    {
        var date = DateTime.UtcNow.Date;
        var key = CacheKeyHelper.RatesKey(Language.EN, date);

        var api = new FakeExchangeRateApiClient
        {
            OnGetExchangeRatesAsync = (d, l, ct) => Task.FromResult<IEnumerable<ExchangeRateApiDto>>(new[] {
                new ExchangeRateApiDto { ValidFor = date.ToString("yyyy-MM-dd"), Amount = 1, CurrencyCode = "USD", Rate = 25m },
                new ExchangeRateApiDto { ValidFor = date.ToString("yyyy-MM-dd"), Amount = 1, CurrencyCode = "EUR", Rate = 26m }
            })
        };

        var cache = new FakeCacheRepository();

        var p = new PerDayProcessor(api, cache, new NullLogger<PerDayProcessor>());

        await p.ProcessDateAsync(date, Language.EN, CancellationToken.None);

        var cached = await cache.GetRatesDictionaryAsync(key);
        cached.ShouldNotBeNull();
        cached.Count.ShouldBe(2);
        cached.ContainsKey("USD").ShouldBeTrue();
        cached.ContainsKey("EUR").ShouldBeTrue();
    }
}
