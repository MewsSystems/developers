using ExchangeRateUpdater.Worker.Services;
using ExchangeRateUpdater.Application.ExchangeRates.Dtos;
using Microsoft.Extensions.Logging.Abstractions;
using UnitTests.Fakers;

namespace UnitTests.Services;

public class CzYearProcessorTests
{
    [Fact]
    public async Task ProcessYearAsync_Returns_Null_When_No_Data()
    {
        var api = new FakeExchangeRateApiClient
        {
            OnGetDefaultExchangeRatesForYearAsync = year => Task.FromResult(new List<ExchangeRateApiDto>())
        };

        var cache = new FakeCacheRepository();
        var logger = new NullLogger<CzYearProcessor>();

        var p = new CzYearProcessor(api, cache, logger);

        var res = await p.ProcessYearAsync(2020, CancellationToken.None);

        res.ShouldBeNull();
    }

    [Fact]
    public async Task ProcessYearAsync_Caches_And_Returns_Earliest_Date()
    {
        var dto1 = new ExchangeRateApiDto { ValidFor = "2020-01-02", Amount = 1, CurrencyCode = "USD", Rate = 25m };
        var dto2 = new ExchangeRateApiDto { ValidFor = "2020-01-01", Amount = 1, CurrencyCode = "EUR", Rate = 26m };
        var api = new FakeExchangeRateApiClient
        {
            OnGetDefaultExchangeRatesForYearAsync = year => Task.FromResult(new List<ExchangeRateApiDto> { dto1, dto2 })
        };

        var cache = new FakeCacheRepository();
        var logger = new NullLogger<CzYearProcessor>();

        var p = new CzYearProcessor(api, cache, logger);

        var res = await p.ProcessYearAsync(2020, CancellationToken.None);

        res.ShouldNotBeNull();
        res.Value.ShouldBe(new DateTime(2020, 1, 1));

        var key1 = $"exrates:cz:{new DateTime(2020,1,1):yyyy-MM-dd}";
        var key2 = $"exrates:cz:{new DateTime(2020,1,2):yyyy-MM-dd}";

        var cached1 = await cache.GetRatesDictionaryAsync(key1);
        var cached2 = await cache.GetRatesDictionaryAsync(key2);

        cached1.ShouldNotBeNull();
        cached1.ContainsKey("EUR").ShouldBeTrue();

        cached2.ShouldNotBeNull();
        cached2.ContainsKey("USD").ShouldBeTrue();
    }

    [Fact]
    public async Task ProcessYearAsync_Skips_Invalid_ValidFor()
    {
        var dto1 = new ExchangeRateApiDto { ValidFor = "", Amount = 1, CurrencyCode = "USD", Rate = 25m };
        var dto2 = new ExchangeRateApiDto { ValidFor = "2020-01-05", Amount = 1, CurrencyCode = "EUR", Rate = 26m };
        var api = new FakeExchangeRateApiClient
        {
            OnGetDefaultExchangeRatesForYearAsync = year => Task.FromResult(new List<ExchangeRateApiDto> { dto1, dto2 })
        };

        var cache = new FakeCacheRepository();
        var logger = new NullLogger<CzYearProcessor>();

        var p = new CzYearProcessor(api, cache, logger);

        var res = await p.ProcessYearAsync(2020, CancellationToken.None);

        res.ShouldNotBeNull();
        res.Value.ShouldBe(new DateTime(2020, 1, 5));

        var key = $"exrates:cz:{res.Value:yyyy-MM-dd}";
        var cached = await cache.GetRatesDictionaryAsync(key);
        cached.ShouldNotBeNull();
        cached.Count.ShouldBe(1);
        cached.ContainsKey("EUR").ShouldBeTrue();
    }
}
