using ExchangeRateUpdater.Domain.Models;
using Xunit;

namespace ExchangeRateUpdater.Domain.UnitTests.Models;

public class CurrencyShould
{
    [Fact]
    public void BeEqual()
    {
        // create using different strings (with same meaning)
        var currency1 = Currency.FromString("USD");
        var currency2 = Currency.FromString("USD");

        Assert.Equal(currency1, currency2);
    }

    [Fact]
    public void NotBeEqual()
    {
        var currency1 = Currency.FromString("USD");
        var currency2 = Currency.FromString("CZK");

        Assert.NotEqual(currency1, currency2);
    }

    [Fact]
    public void NotEqualToNull()
    {
        var currency1 = Currency.FromString("USD");
        var currency2 = (Currency?)null;

        Assert.NotEqual(currency1, currency2);
    }

    [Theory]
    [MemberData(nameof(GenerateValidValues))]
    public void ConvertFromStringSuccessfully(string currencyString, Currency expected)
    {
        var culture = Currency.FromString(currencyString);
        Assert.Equal(expected, culture);
    }

    [Fact]
    public void ThrowOnNull() =>
        Assert.Throws<ArgumentNullException>(() => Currency.FromString(null!));

    public static IEnumerable<object[]> GenerateValidValues()
    {
        // different casing
        yield return new object[] { "USD", Currency.FromString("usd") };
        yield return new object[] { "USD", Currency.FromString("USD") };
        yield return new object[] { "USD", Currency.FromString("uSd") };
    }
}
