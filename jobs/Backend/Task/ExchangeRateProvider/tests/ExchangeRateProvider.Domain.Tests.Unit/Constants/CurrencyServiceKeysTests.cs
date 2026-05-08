using ExchangeRateProvider.Domain.Constants;
using Shouldly;

namespace ExchangeRateProvider.Domain.Tests.Unit.Constants;

public class CurrencyServiceKeysTests
{
    [Theory]
    [InlineData("CZK")]
    [InlineData("czk")]
    [InlineData("Czk")]
    public void IsSupported_WithValidCurrency_ReturnsTrue(string currencyCode)
    {
        CurrencyServiceKeys.IsSupported(currencyCode).ShouldBeTrue();
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("XXX")]
    public void IsSupported_WithUnsupportedCurrency_ReturnsFalse(string currencyCode)
    {
        CurrencyServiceKeys.IsSupported(currencyCode).ShouldBeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void IsSupported_WithNullOrWhitespace_ReturnsFalse(string currencyCode)
    {
        CurrencyServiceKeys.IsSupported(currencyCode).ShouldBeFalse();
    }

    [Fact]
    public void GetSupportedCurrencies_ReturnsAllSupportedCurrencies()
    {
        var result = CurrencyServiceKeys.GetSupportedCurrencies();

        result.ShouldNotBeEmpty();
        result.ShouldContain("CZK");
    }

    [Fact]
    public void Default_IsCZK()
    {
        CurrencyServiceKeys.Default.ShouldBe("CZK");
    }
}