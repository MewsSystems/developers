using ExchangeRateError;
using ExchangeRateService.Cache;
using ExchangeRateService.CNB.Client.Interfaces;
using ExchangeRateService.CNB.Provider;
using Moq;

namespace ExchangeRateServiceTest.Provider;

public class CNBExchangeRateProviderTest
{
    private readonly Currency _testEURCurrency = new Currency("EUR");
    private readonly Currency _testUSDCurrency = new Currency("USD");
    private readonly Currency _testCZKCurrency = new Currency("CZK");
    private readonly DateTime _time = DateTime.Now;
    private ExchangeRate _testExchangeRate1;

    [SetUp]
    public void Setup()
    {
        _testExchangeRate1 = new ExchangeRate(_testEURCurrency, _testCZKCurrency, 0, _time);
    }

    [Test]
    public async Task GetExchangeRate()
    {
        var logger = NullLogger<CNBExchangeRateProvider>.Instance;
        var mockCache = new Mock<IExchangeRateCache>();
        var mockClient = new Mock<ICNBClient>();
        
        mockClient.Setup(s => s.TargetCurrency).Returns(_testCZKCurrency);
        mockClient.Setup(s => s.GetExchangeRates(It.IsAny<IList<Currency>>(), It.IsAny<DateTime>()))
            .ReturnsAsync([_testExchangeRate1]);
        
        var provider = new CNBExchangeRateProvider(logger, mockClient.Object, mockCache.Object);
        
        var result = await provider.GetExchangeRate(_testEURCurrency, _time);
        
        Assert.That(result, Is.EqualTo(_testExchangeRate1));
    }
    
    [Test]
    public async Task GetExchangeRateCached()
    {
        var logger = NullLogger<CNBExchangeRateProvider>.Instance;
        var mockCache = new Mock<IExchangeRateCache>();
        var mockClient = new Mock<ICNBClient>();
        
        mockCache.Setup(s => s.TryGetExchangeRate(It.IsAny<ExchangeRate>(), out _testExchangeRate1!))
            .ReturnsAsync(true);
        
        mockClient.Setup(s => s.TargetCurrency).Returns(_testCZKCurrency);
        
        var provider = new CNBExchangeRateProvider(logger, mockClient.Object, mockCache.Object);
        
        var result = await provider.GetExchangeRate(_testEURCurrency, _time);
        
        Assert.That(result, Is.EqualTo(_testExchangeRate1));
        
        mockClient.Verify(x => x.GetExchangeRates(It.IsAny<IList<Currency>>(), It.IsAny<DateTime>()),
            Times.Never);
    }
    
    [Test]
    public async Task GetExchangeRateCachedAfterUse()
    {
        var logger = NullLogger<CNBExchangeRateProvider>.Instance;
        var mockCache = new Mock<IExchangeRateCache>();
        var mockClient = new Mock<ICNBClient>();

        mockCache.SetupSequence(s => s.TryGetExchangeRate(It.IsAny<ExchangeRate>(), out _testExchangeRate1!))
            .ReturnsAsync(false)
            .ReturnsAsync(true);
        
        mockClient.Setup(s => s.TargetCurrency).Returns(_testCZKCurrency);
        mockClient.Setup(s => s.GetExchangeRates(It.IsAny<IList<Currency>>(), It.IsAny<DateTime>()))
            .ReturnsAsync([_testExchangeRate1]);
        
        var provider = new CNBExchangeRateProvider(logger, mockClient.Object, mockCache.Object);
        
        var firstResult = await provider.GetExchangeRate(_testEURCurrency, _time);
        var secondResult = await provider.GetExchangeRate(_testEURCurrency, _time);
        
        Assert.That(firstResult, Is.EqualTo(_testExchangeRate1));
        Assert.That(secondResult, Is.EqualTo(_testExchangeRate1));
        
        mockClient.Verify(x => x.GetExchangeRates(It.IsAny<IList<Currency>>(), It.IsAny<DateTime>()),
            Times.Once);

    }
    
    [Test]
    public void GetExchangeRateMissing()
    {
        var logger = NullLogger<CNBExchangeRateProvider>.Instance;
        var mockCache = new Mock<IExchangeRateCache>();
        var mockClient = new Mock<ICNBClient>();
        
        mockClient.Setup(s => s.TargetCurrency).Returns(_testCZKCurrency);
        mockClient.Setup(s => s.GetExchangeRates(It.IsAny<IList<Currency>>(), It.IsAny<DateTime>()))
            .ReturnsAsync([_testExchangeRate1]);
        
        var provider = new CNBExchangeRateProvider(logger, mockClient.Object, mockCache.Object);
        
        Assert.ThrowsAsync<ExchangeRateException>(async () => await provider.GetExchangeRate(_testUSDCurrency, _time));
    }

    
}