using ExchangeRateUpdater.Application.ExchangeRates.Query.GetExchangeRatesDaily;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace UnitTests.Handlers;

using ExchangeRateUpdater.Application.Common.Utils;
using ExchangeRateUpdater.Domain.Enums;
using Fakers;

public class GetExchangesRatesByDateQueryHandlerTests
{
    [Fact]
    public async Task Handle_Returns_Only_Requested_SourceCurrencies()
    {
        var date = DateTime.UtcNow.Date;

        var key = CacheKeyHelper.RatesKey(Language.CZ, date);

        var existing = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 24.5m),
            new(new Currency("EUR"), new Currency("CZK"), 26m),
            new(new Currency("GBP"), new Currency("CZK"), 29m)
        };

        var cache = new FakeCacheRepository();
        await cache.SetRatesListAsync(key, existing);

        var handler = new GetExchangesRatesByDateQueryHandler(cache);

        var q = new GetExchangesRatesByDateQuery { CurrencyCodes = new List<string> { "USD", "GBP" }, Date = date };

        var result = await handler.Handle(q, default);

        result.Count.ShouldBe(2);
        result.Any(r => r.SourceCurrencyCode == "USD").ShouldBeTrue();
        result.Any(r => r.SourceCurrencyCode == "GBP").ShouldBeTrue();
    }

    [Fact]
    public async Task Handle_Uses_Defaults_When_Null_Date_And_Language()
    {
        var date = DateTime.UtcNow.Date;

        var key = CacheKeyHelper.RatesKey(Language.CZ, date);

        var existing = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 24.5m)
        };

        var cache = new FakeCacheRepository();
        await cache.SetRatesListAsync(key, existing);

        var handler = new GetExchangesRatesByDateQueryHandler(cache);

        var q = new GetExchangesRatesByDateQuery { CurrencyCodes = new List<string> { "USD" } };

        var result = await handler.Handle(q, default);

        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Handle_Returns_Empty_When_No_Matching_SourceCurrency()
    {
        var date = DateTime.UtcNow.Date;

        var key = CacheKeyHelper.RatesKey(Language.CZ, date);

        var existing = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 26m)
        };

        var cache = new FakeCacheRepository();
        await cache.SetRatesListAsync(key, existing);

        var handler = new GetExchangesRatesByDateQueryHandler(cache);

        var q = new GetExchangesRatesByDateQuery { CurrencyCodes = new List<string> { "USD" }, Date = date };

        var result = await handler.Handle(q, default);

        result.ShouldBeEmpty();
    }
}
