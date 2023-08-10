using ExchangeRateUpdater.Dto;

namespace ExchangeRateUpdater.Test;

public class Tests
{
    ExchangeRateProvider _exchangeRateProvider = null!;
    readonly Mock<ICurrencyValidator> _currencyValidatorMock = new();
    readonly Mock<IExchangeRateFetcher> _exchangeRateFetcherMock = new();
    
    [SetUp]
    public void Setup()
    {
        _currencyValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        _exchangeRateProvider =
            new ExchangeRateProvider(_currencyValidatorMock.Object, _exchangeRateFetcherMock.Object);
    }

    [Test]
    public void WhenCurrencyNotValid_ShouldThrowException()
    {
        _currencyValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
        Assert.ThrowsAsync<ArgumentException>(async () => await _exchangeRateProvider.GetExchangeRates(new []{new Currency("SomeCurrency")}));
    }
    
    [Test]
    public async Task WhenCurrencyValid_ShouldReturnRates()
    {
        _exchangeRateFetcherMock
            .Setup(x => x.FetchCurrentAsync())
            .ReturnsAsync(new ExchangeRatesBo(
                new []{new ExchangeRateBo("EUR", 10.5m, 2)}));
        
        (await _exchangeRateProvider.GetExchangeRates(new []{new Currency("EUR")}))
            .Should()
            .BeEquivalentTo(new [] {new ExchangeRate("CZK", "EUR", 10.5m / 2)});
        
    }
}