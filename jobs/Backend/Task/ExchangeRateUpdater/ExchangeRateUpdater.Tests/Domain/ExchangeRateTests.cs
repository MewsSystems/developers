using Xunit;
using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Tests.Domain;

public class ExchangeRateTests
{
    [Fact]
    public void Constructor_NormalizesCurrencyCode()
    {
        var rate = new ExchangeRate("eur", 24.170m, DateTime.UtcNow);
        Assert.Equal("EUR", rate.CurrencyCode);
    }

    [Theory] // Tests for various rounding scenarios
    [InlineData(24.17012345, 24.1701)]  // Rounds to 4 decimals
    [InlineData(24.1709, 24.1709)] // Correct value stays the same
    public void Constructor_RoundsRate(decimal input, decimal expected)
    {
        var rate = new ExchangeRate("USD", input, DateTime.UtcNow);
        Assert.Equal(expected, rate.RateToCZK);
    }
}
