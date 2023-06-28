using ExchangeRateUpdater.Models;
using FluentAssertions;

namespace ExchangeRateUpdater.Tests.Models;

public class CurrencyTests
{
    [Fact]
    public void Equals_ReturnsTrue()
    {
        // Arrange
        var usd = new Currency("USD");
        var usd2 = new Currency("USD");
        
        // Act + Assert
        usd.Equals(usd2).Should().BeTrue();
    }
    
    [Fact]
    public void Equals_ReturnsFalse()
    {
        // Arrange
        var usd = new Currency("USD");
        var nzd = new Currency("NZD");
        
        // Act + Assert
        usd.Equals(nzd).Should().BeFalse();
    }
}