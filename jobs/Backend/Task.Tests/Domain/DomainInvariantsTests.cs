using ExchangeRateUpdater.Domain;
using Xunit;

namespace Task.Tests.Domain;

public sealed class DomainInvariantsTests
{
    [Fact]
    public void Currency_normalizes_code()
    {
        Currency currency = new(" usd ");

        Assert.Equal("USD", currency.Code);
    }

    [Fact]
    public void Currency_empty_or_whitespace_throws()
    {
        Assert.Throws<ArgumentException>(() => new Currency(""));
        Assert.Throws<ArgumentException>(() => new Currency("   "));
    }

    [Fact]
    public void ExchangeRate_requires_non_null_currencies()
    {
        Currency czk = new("CZK");

        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(null!, czk, 1m));
        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(new Currency("USD"), null!, 1m));
    }

    [Fact]
    public void ExchangeRate_requires_positive_value()
    {
        Currency usd = new("USD");
        Currency czk = new("CZK");

        Assert.Throws<ArgumentOutOfRangeException>(() => new ExchangeRate(usd, czk, 0m));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ExchangeRate(usd, czk, -1m));
    }
}
