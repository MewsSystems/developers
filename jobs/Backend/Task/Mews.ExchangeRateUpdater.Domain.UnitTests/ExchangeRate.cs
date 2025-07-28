using Mews.ExchangeRateUpdater.Domain.ValueObjects;

namespace Mews.ExchangeRateUpdater.Domain.UnitTests;


public class ExchangeRateTests
{
    [Fact]
    public void Constructor_ShouldCreateExchangeRate_WhenParametersAreValid()
    {
        // Arrange
        var source = new Currency("USD");
        var target = new Currency("CZK");
        var value = 22.345m;

        // Act
        var rate = new ExchangeRate(source, target, value);

        // Assert
        Assert.Equal(source, rate.SourceCurrency);
        Assert.Equal(target, rate.TargetCurrency);
        Assert.Equal(value, rate.Value);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenAllFieldsMatch()
    {
        // Arrange
        var r1 = new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.123m);
        var r2 = new ExchangeRate(new Currency("eur"), new Currency("czk"), 24.123m);

        // Assert
        Assert.Equal(r1, r2);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenAnyFieldDiffers()
    {
        // Arrange
        var baseCurrency = new Currency("EUR");
        var czk = new Currency("CZK");
        var usd = new Currency("USD");

        var r1 = new ExchangeRate(baseCurrency, czk, 24.123m);
        var r2 = new ExchangeRate(baseCurrency, czk, 24.120m); // different value
        var r3 = new ExchangeRate(baseCurrency, usd, 24.123m); // different target
        var r4 = new ExchangeRate(usd, czk, 24.123m);           // different source

        // Assert
        Assert.NotEqual(r1, r2);
        Assert.NotEqual(r1, r3);
        Assert.NotEqual(r1, r4);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var rate = new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.345m);

        // Act
        var str = rate.ToString();

        // Assert
        Assert.Equal("USD/CZK=22.345", str);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ShouldThrow_WhenValueIsZeroOrNegative(decimal value)
    {
        // Arrange
        var source = new Currency("USD");
        var target = new Currency("CZK");

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new ExchangeRate(source, target, value));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenCurrenciesAreTheSame()
    {
        // Arrange
        var currency = new Currency("USD");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ExchangeRate(currency, currency, 1.0m));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenSourceIsNull()
    {
        // Arrange
        var target = new Currency("CZK");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(null!, target, 1.0m));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenTargetIsNull()
    {
        // Arrange
        var source = new Currency("USD");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(source, null!, 1.0m));
    }
}