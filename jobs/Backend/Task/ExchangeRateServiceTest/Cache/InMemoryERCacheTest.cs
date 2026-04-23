using ExchangeRateService.Cache;

namespace ExchangeRateServiceTest.Cache;

public class InMemoryERCacheTest
{
    
    private readonly ExchangeRate _testExchangeRate1 = new (new Currency("EUR"), new Currency("USD"), 0, DateTime.Now);
    private readonly ExchangeRate _testExchangeRate2 = new (new Currency("EUR"), new Currency("USD"), 0, DateTime.Now.AddDays(-1));
    private InMemoryERCache _cache;
    [SetUp]
    public void Setup()
    {
        var logger = NullLogger<InMemoryERCache>.Instance;
        _cache = new InMemoryERCache(logger);
    }
    
    [Test]
    public void AddExchangeRateTest()
    {
        Assert.DoesNotThrowAsync(() => _cache.AddExchangeRate(_testExchangeRate1));
    }
    
    [Test]
    public void MultipleAddExchangeRateTest()
    {
        Assert.DoesNotThrowAsync(() => _cache.AddExchangeRate(_testExchangeRate1));
        Assert.DoesNotThrowAsync(() => _cache.AddExchangeRate(_testExchangeRate1));
        Assert.DoesNotThrowAsync(() => _cache.AddExchangeRate(_testExchangeRate2));
    }

    [Test]
    public void AddExchangeRatesTest()
    {
        Assert.DoesNotThrowAsync(() => _cache.AddExchangeRates([_testExchangeRate1, _testExchangeRate2]));
    }

    [Test]
    public async Task EmptyTryGetExchangeRate()
    {
        Assert.IsFalse(await _cache.TryGetExchangeRate(_testExchangeRate1, out var _));
        Assert.IsFalse(await _cache.TryGetExchangeRate(_testExchangeRate1, out var _));
        Assert.IsFalse(await _cache.TryGetExchangeRate(_testExchangeRate2, out var _));
    }

    [Test]
    public async Task DifferentTryGetExchangeRate()
    {
        Assert.DoesNotThrowAsync(() => _cache.AddExchangeRate(_testExchangeRate1));
        
        Assert.IsFalse(await _cache.TryGetExchangeRate(_testExchangeRate2, out var _));
    }

    [Test]
    public async Task TryGetExchangeRates()
    {
        Assert.DoesNotThrowAsync(() => _cache.AddExchangeRate(_testExchangeRate1));
        
        Assert.IsTrue(await _cache.TryGetExchangeRate(_testExchangeRate1, out var _));
    }
    
}