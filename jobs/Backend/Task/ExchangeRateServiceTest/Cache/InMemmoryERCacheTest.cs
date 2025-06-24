using ExchangeRateService.Cache;

namespace ExchangeRateServiceTest.Cache;

public class InMemmoryERCacheTest
{
    
    private readonly ExchangeRate _testExchangeRate1 = new (new Currency("EUR"), new Currency("USD"), 0, DateTime.Now);
    private readonly ExchangeRate _testExchangeRate2 = new (new Currency("EUR"), new Currency("USD"), 0, DateTime.Now.AddDays(-1));
    private InMemmoryERCache _cache;
    [SetUp]
    public void Setup()
    {
        var logger = NullLogger<InMemmoryERCache>.Instance;
        _cache = new InMemmoryERCache(logger);
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
        Assert.IsNull(await _cache.TryGetExchangeRate(_testExchangeRate1));
        Assert.IsNull(await _cache.TryGetExchangeRate(_testExchangeRate1));
        Assert.IsNull(await _cache.TryGetExchangeRate(_testExchangeRate2));
    }

    [Test]
    public async Task DifferentTryGetExchangeRate()
    {
        Assert.DoesNotThrowAsync(() => _cache.AddExchangeRate(_testExchangeRate1));
        
        Assert.IsNull(await _cache.TryGetExchangeRate(_testExchangeRate2));
    }

    [Test]
    public async Task TryGetExchangeRates()
    {
        Assert.DoesNotThrowAsync(() => _cache.AddExchangeRate(_testExchangeRate1));
        
        Assert.IsNotNull(await _cache.TryGetExchangeRate(_testExchangeRate1));
    }
    
}