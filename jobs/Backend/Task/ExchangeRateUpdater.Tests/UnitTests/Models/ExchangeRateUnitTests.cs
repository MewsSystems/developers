using Xunit;
using ExchangeRateUpdater;

public class ExchangeRateUnitTests
{
    [Fact]
    public void Constructor_WithValidArguments_CreatesInstanceWithPropertiesSet()
    {
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");
        decimal rateValue = 0.89m;
        
        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, rateValue);
        
        Assert.Equal(sourceCurrency, exchangeRate.SourceCurrency);
        Assert.Equal(targetCurrency, exchangeRate.TargetCurrency);
        Assert.Equal(rateValue, exchangeRate.Value);
    }

    [Fact]
    public void ToString_WhenCalled_ReturnsFormattedString()
    {
        var exchangeRate = new ExchangeRate(new Currency("GBP"), new Currency("USD"), 1.35m);
        
        var result = exchangeRate.ToString();

        Assert.Equal("GBP/USD=1.35", result);
    }

}