using ExchangeRateUpdater.Entities;
using Shouldly;

namespace ExchangeRateUpdater.Tests.Entities;

public class CurrencyTests
{
    [Fact]
    public void Constructor_SetsCodeProperly()
    {
        // Arrange
        string expectedCode = "USD";

        // Act
        Currency currency = new(expectedCode);

        // Assert
        currency.Code.ShouldBe(expectedCode);
    }

    [Fact]
    public void ToString_ReturnsCurrencyCode()
    {
        // Arrange
        Currency currency = new("EUR");

        // Act
        string result = currency.ToString();

        // Assert
        result.ShouldBe("EUR");
    }
}
