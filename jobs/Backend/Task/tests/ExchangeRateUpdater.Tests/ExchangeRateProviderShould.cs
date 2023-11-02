namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderShould
{
    [Fact]
    public void ReturnExchangeRates()
    {
        // act
        var provider = new ExchangeRateProvider();
        var rates = provider.GetExchangeRates(new[] { new Currency("EUR") });

        // assert
        var rate = Assert.Single(rates);
        Assert.Equal("EUR", rate.SourceCurrency.Code);
        Assert.Equal("CZK", rate.TargetCurrency.Code);
        
        // let's expect that the exchange rate is positive (⊙_⊙;)
        Assert.True(rate.Value > 0);
    }
    
    [Fact]
    public void NotReturnUnknownCurrency()
    {
        // act
        var provider = new ExchangeRateProvider();
        var rates = provider.GetExchangeRates(new[] { new Currency("SPL") });

        // assert
        // SPL – Seborga Luigino (Principality of Seborga) is not expected to be supported by the CNB
        Assert.Empty(rates);
    }
}