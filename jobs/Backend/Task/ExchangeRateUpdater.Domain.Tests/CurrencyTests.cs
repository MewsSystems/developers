namespace ExchangeRateUpdater.Domain.Tests;

public class CurrencyTests
{
    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("CZK")]
    [InlineData("JPY")]
    [InlineData("KES")]
    [InlineData("RUB")]
    [InlineData("THB")]
    [InlineData("TRY")]
    public void Constructor_When_code_is_valid_Then_currency_is_created(string code)
    {
        //Act
        var currency = new Currency(code);
        
        //Assert
        Assert.NotNull(currency);
    }

    [Fact]
    public void Constructor_When_code_is_invalid_Then_exception_is_thrown()
    {
        //Arrange
        const string invalidCode = "XYZ";
        
        //Act
        var action = () => new Currency(invalidCode);
        
        //Assert
        Assert.Throws<ArgumentException>(action);
    }
}