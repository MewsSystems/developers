namespace ExchangeRateUpdater.Tests;

public class CurrencyTests
{
    [Fact]
    public void ShouldInitNewCurrency()
    {
        var curr = new Currency("USD");
        Assert.NotNull(curr);
        Assert.Equal("USD", curr.Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    [InlineData(" ")]
    public void ShouldThrowArgumentExceptionWhenCodeIsNullOrWhitespace(string code)
    {
        Assert.Throws<ArgumentNullException>(() => new Currency(code));
    }

    [Theory]
    [InlineData("USDC")]
    [InlineData("US")]
    public void ShouldThrowIfInvalidLength(string code)
    {
        Assert.Throws<ArgumentException>(() => new Currency(code));
    }

    [Theory]
    [InlineData("usd")]
    [InlineData("Usd")]
    public void ShouldThrowIfCaseMismatch(string code)
    {
        Assert.Throws<ArgumentException>(() => new Currency(code));
    }

    [Theory]
    [InlineData("AB1")]
    [InlineData("AR%")]
    public void ShouldThrowIfNotAllLetters(string code)
    {
        Assert.Throws<ArgumentException>(() => new Currency(code));
    }
}
