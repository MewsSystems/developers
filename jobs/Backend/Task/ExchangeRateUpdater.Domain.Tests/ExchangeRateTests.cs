namespace ExchangeRateUpdater.Domain.Tests;

public class ExchangeRateTests
{
    [Fact]
    public void ToString_When_requested_Then_it_is_printed_as_expected()
    {
        //Arrange
        var sourceCurrency = new Currency("EUR");
        var targetCurrency = new Currency("USD");
        const decimal value = 3.999m;
        var sut = new ExchangeRate(sourceCurrency, targetCurrency, value);
        
        //Act
        var result = sut.ToString();
        
        //Assert
        Assert.Equal($"{sourceCurrency}/{targetCurrency}={value}", result);
    }
}