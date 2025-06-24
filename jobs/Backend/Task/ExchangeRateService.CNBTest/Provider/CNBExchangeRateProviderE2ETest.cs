using ExchangeRateService.Cache;
using ExchangeRateService.CNB.Client;
using ExchangeRateService.CNB.Client.Interfaces;
using ExchangeRateService.CNB.Provider;

namespace ExchangeRateServiceTest.Provider;

public class CNBExchangeRateProviderE2ETest
{
    
    private static readonly Currency EURCurrency = new Currency("EUR");
    private static readonly Currency USDCurrency = new Currency("USD");
    private static readonly Currency CZKCurrency = new Currency("CZK");
    private static readonly Currency BBDCurrency = new Currency("BBD");
    private static readonly DateTime Date = DateTime.Parse("2025-06-01");
    private static readonly ExchangeRate _EURToCZKRate = new ExchangeRate(EURCurrency, CZKCurrency, 24.930m, Date);
    
    private ICNBClient _client;
    private IExchangeRateCache _cache;
    private CNBExchangeRateProvider _provider;
    
    [SetUp]
    public void Setup()
    {
        var _clientLogger = NullLogger<CNBClient>.Instance;
        var _cacheLogger = NullLogger<InMemoryERCache>.Instance;
        var _provierLogger = NullLogger<CNBExchangeRateProvider>.Instance;
        
        _client = new CNBClient(_clientLogger);
        _cache = new InMemoryERCache(_cacheLogger);
        _provider = new CNBExchangeRateProvider(_provierLogger, _client, _cache);
        
    }
    
    [Test]
    public async Task GetExchangeRateTest()
    {
        
        var result = await _provider.GetExchangeRate(EURCurrency, Date);
        
        Assert.That(result, Is.EqualTo(_EURToCZKRate));
    }
    
    [Test]
    public async Task GetExchangeRateTestMultiple()
    {
        var result = await _provider.GetExchangeRate(EURCurrency, Date);

        Assert.IsTrue(await _cache.TryGetExchangeRate(_EURToCZKRate, out var cacheHit));
        
        // this should cache the result value
        Assert.That(result, Is.EqualTo(_EURToCZKRate));

        Assert.NotNull(cacheHit);

    }
    
    [Test]
    public async Task GetExchangeRateTestDifferentAfterCache()
    {
        var BBDToCZKRate = new ExchangeRate(BBDCurrency, CZKCurrency, 0, Date);
        var BBDToCZKRateOld = new ExchangeRate(BBDCurrency, CZKCurrency, 0, Date.AddDays(-1));
        
        var result = await _provider.GetExchangeRate(EURCurrency, Date);
        
        Assert.IsTrue(await _cache.TryGetExchangeRate(_EURToCZKRate, out var cacheHit));
        Assert.IsTrue(await _cache.TryGetExchangeRate(BBDToCZKRate, out var bbdHit));
        Assert.IsFalse(await _cache.TryGetExchangeRate(BBDToCZKRateOld, out var _));
        
        // this should cache the result value
        Assert.That(result, Is.EqualTo(_EURToCZKRate));

        Assert.NotNull(cacheHit);
        Assert.NotNull(bbdHit);

    }


    
}