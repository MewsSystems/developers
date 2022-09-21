using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Tests.Mocks;
using Moq;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    private readonly Mock<IExchangeRateParser> _parserMock;
    
    private readonly ExchangeRateProvider _provider;

    public ExchangeRateProviderTests()
    {
        _parserMock = new Mock<IExchangeRateParser>();
        _provider = new ExchangeRateProvider(new ExchangeRateLoaderMock(), _parserMock.Object);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_NoCurrencies_NoResults()
    {
        _parserMock.Setup(a => a.ParseAsync(It.IsAny<Stream>())).ReturnsAsync(Enumerable.Empty<ExchangeRate>());
        
        var result = await _provider.GetExchangeRatesAsync(Enumerable.Empty<Currency>());
        
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetExchangeRatesAsync_SomeCurrencies_CorrectResults()
    {
        _parserMock.Setup(a => a.ParseAsync(It.IsAny<Stream>())).ReturnsAsync(new []
        {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 1),
            new ExchangeRate(new Currency("CZK"), new Currency("EUR"), 1),
            new ExchangeRate(new Currency("CZK"), new Currency("JPY"), 1),
        });
        
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
        };
        var result = await _provider.GetExchangeRatesAsync(currencies);
        
        Assert.Equal(2, result.Count());
    }
    
    [Fact]
    public async Task GetExchangeRatesAsync_CacheHit_Success()
    {
        _parserMock.Setup(a => a.ParseAsync(It.IsAny<Stream>())).ReturnsAsync(new []
        {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 1),
            new ExchangeRate(new Currency("CZK"), new Currency("EUR"), 1),
            new ExchangeRate(new Currency("CZK"), new Currency("JPY"), 1),
        });

        var currencies = new[] { new Currency("USD") };
        await _provider.GetExchangeRatesAsync(currencies);
        await _provider.GetExchangeRatesAsync(currencies);
        
        _parserMock.Verify(a => a.ParseAsync(It.IsAny<Stream>()), Times.Exactly(1));
    }
}