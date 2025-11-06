using ExchangeRateUpdater.Abstractions.Model;
using ExchangeRateUpdater.CnbClient.Implementation;

namespace ExchangeRateUpdater.Tests.CnbClient.Implementation;

[TestFixture]
[TestOf(typeof(InMemoryExchangeRateCache))]
public class InMemoryExchangeRateCacheTest
{
    [Test]
    public void IsEmpty_ReturnsTrue_WhenCacheIsNew()
    {
        var cache = new InMemoryExchangeRateCache();
        Assert.That(cache.IsEmpty(), Is.True);
    }

    [Test]
    public void Set_AddsValue_AndIsEmptyReturnsFalse()
    {
        var cache = new InMemoryExchangeRateCache();
        var value = new CurrencyValue { CurrencyCode = "USD", Amount = 1, ValidFor = DateTime.UtcNow, Rate = 23.5m };
        cache.Set("USD", value);
        Assert.That(cache.IsEmpty(), Is.False);
    }

    [Test]
    public void GetAll_ReturnsAllValues()
    {
        var cache = new InMemoryExchangeRateCache();
        var value1 = new CurrencyValue { CurrencyCode = "USD", Amount = 1, ValidFor = DateTime.UtcNow, Rate = 23.5m };
        var value2 = new CurrencyValue { CurrencyCode = "EUR", Amount = 1, ValidFor = DateTime.UtcNow, Rate = 25.0m };
        cache.Set("USD", value1);
        cache.Set("EUR", value2);
        var all = cache.GetAll();
        Assert.That(all, Has.Count.EqualTo(2));
        Assert.That(all, Does.Contain(value1));
        Assert.That(all, Does.Contain(value2));
    }

    [Test]
    public void Set_OverwritesValue_ForSameKey()
    {
        var cache = new InMemoryExchangeRateCache();
        var value1 = new CurrencyValue { CurrencyCode = "USD", Amount = 1, ValidFor = DateTime.UtcNow, Rate = 23.5m };
        var value2 = new CurrencyValue { CurrencyCode = "USD", Amount = 1, ValidFor = DateTime.UtcNow, Rate = 24.0m };
        cache.Set("USD", value1);
        cache.Set("USD", value2);
        var all = cache.GetAll();
        Assert.That(all, Has.Count.EqualTo(1));
        Assert.That(all[0].Rate, Is.EqualTo(24.0m));
    }
}