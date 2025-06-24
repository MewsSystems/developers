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
    private static readonly DateTime Date = DateTime.Parse("2025-05-30");
    private static readonly ExchangeRate _EURToCZKRate = new ExchangeRate(EURCurrency, CZKCurrency, 24.930m, Date);
    
    private ICNBClient _client;
    private IExchangeRateCache _cache;
    private CNBExchangeRateProvider _provider;
    
    [SetUp]
    public void Setup()
    {
        var _clientLogger = NullLogger<CNBClient>.Instance;
        var _cacheLogger = NullLogger<InMemmoryERCache>.Instance;
        var _provierLogger = NullLogger<CNBExchangeRateProvider>.Instance;
        
        _client = new CNBClient(_clientLogger);
        _cache = new InMemmoryERCache(_cacheLogger);
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

        var cacheHit = await _cache.TryGetExchangeRate(_EURToCZKRate);
        
        // this should cache the result value
        Assert.That(result, Is.EqualTo(_EURToCZKRate));

        Assert.NotNull(cacheHit);

    }

    
}