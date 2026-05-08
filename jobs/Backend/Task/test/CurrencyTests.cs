using ExchangeRateUpdater;

namespace ExchangeRateUpdater.Tests;

public class CurrencyTests
{
    [Fact]
    public void Constructor_ValidCode_SetsCode()
    {
        // Arrange
        var code = "USD";

        // Act
        var currency = new Currency(code);

        // Assert
        Assert.Equal("USD", currency.Code);
    }

    [Fact]
    public void Constructor_LowercaseCode_ConvertsToUppercase()
    {
        // Arrange
        var code = "usd";

        // Act
        var currency = new Currency(code);

        // Assert
        Assert.Equal("USD", currency.Code);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_NullOrEmptyCode_ThrowsArgumentException(string? code)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Currency(code!));
    }

    [Theory]
    [InlineData("US")]
    [InlineData("USDD")]
    [InlineData("A")]
    [InlineData("ABCD")]
    public void Constructor_InvalidLength_ThrowsArgumentException(string code)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Currency(code));
        Assert.Contains("3 characters", exception.Message);
    }

    [Fact]
    public void ToString_ReturnsCode()
    {
        // Arrange
        var currency = new Currency("USD");

        // Act & Assert
        Assert.Equal("USD", currency.ToString());
    }
}
