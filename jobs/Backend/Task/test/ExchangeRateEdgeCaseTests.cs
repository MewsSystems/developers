using ExchangeRateUpdater;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateEdgeCaseTests
{
    [Fact]
    public void Constructor_ZeroValue_ThrowsException()
    {
        // Arrange
        var source = new Currency("CZK");
        var target = new Currency("USD");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ExchangeRate(source, target, 0m));
        Assert.Contains("greater than zero", exception.Message);
    }

    [Fact]
    public void Constructor_NegativeValue_ThrowsException()
    {
        // Arrange
        var source = new Currency("CZK");
        var target = new Currency("USD");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ExchangeRate(source, target, -1m));
        Assert.Contains("greater than zero", exception.Message);
    }

    [Fact]
    public void Constructor_NullSourceCurrency_ThrowsException()
    {
        // Arrange
        var target = new Currency("USD");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(null!, target, 24.5m));
    }

    [Fact]
    public void Constructor_NullTargetCurrency_ThrowsException()
    {
        // Arrange
        var source = new Currency("CZK");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(source, null!, 24.5m));
    }

    [Fact]
    public void Constructor_VerySmallValue_Works()
    {
        // Arrange
        var source = new Currency("CZK");
        var target = new Currency("VND");

        // Act
        var rate = new ExchangeRate(source, target, 0.0001m);

        // Assert
        Assert.Equal(0.0001m, rate.Value);
    }

    [Fact]
    public void Constructor_VeryLargeValue_Works()
    {
        // Arrange
        var source = new Currency("CZK");
        var target = new Currency("IRR");

        // Act
        var rate = new ExchangeRate(source, target, 999999.99m);

        // Assert
        Assert.Equal(999999.99m, rate.Value);
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        // Arrange
        var source = new Currency("CZK");
        var target = new Currency("USD");
        var rate = new ExchangeRate(source, target, 24.5m);

        // Act
        var result = rate.ToString();

        // Assert
        Assert.Equal("CZK/USD=24.5", result);
    }

    [Fact]
    public void ToString_WithDecimalPlaces_IncludesDecimals()
    {
        // Arrange
        var source = new Currency("CZK");
        var target = new Currency("JPY");
        var rate = new ExchangeRate(source, target, 0.13425m);

        // Act
        var result = rate.ToString();

        // Assert
        Assert.Contains("0.13425", result);
    }
}

