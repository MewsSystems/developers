using Xunit;
using ExchangeRateUpdater;

public class CurrencyUnitTests
{
    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("JPY")]
    public void Constructor_WhenCalledWithValidCode_AssignsCode(string code)
    {
        var currency = new Currency(code);
        
        Assert.Equal(code, currency.Code);
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    public void ToString_WhenCalled_ReturnsCurrencyCode(string code)
    {
        var currency = new Currency(code);
        
        var result = currency.ToString();
        
        Assert.Equal(code, result);
    }
}