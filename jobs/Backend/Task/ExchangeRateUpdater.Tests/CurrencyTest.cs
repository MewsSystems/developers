using System;
using ExchangeRateUpdater.Domain;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class CurrencyTest
{
    [Theory]
    [InlineData("")]
    [InlineData("CZ")]
    [InlineData("CZK1")]
    public void CreateCurrency_WhenCurrencyIsInvalid_ShouldThrowException(string code)
    {
        Assert.Throws<Exception>(() => new Currency(code));
    }

    [Theory]
    [InlineData("CZK", "CZK")]
    [InlineData("EUR", "EUR")]
    public void CompareCurrency_WhenCurrenciesAreSame_ShouldReturnTrue(string code1, string code2)
    {
        var currency1 = new Currency(code1);
        var currency2 = new Currency(code2);

        Assert.Equal(currency1, currency2);
    }

    [Theory]
    [InlineData("CZK", "EUR")]
    [InlineData("CZK", "USD")]
    public void CompareCurrency_WhenCurrenciesAreDifferent_ShouldReturnFalse(string code1, string code2)
    {
        var currency1 = new Currency(code1);
        var currency2 = new Currency(code2);

        Assert.NotEqual(currency1, currency2);
    }
}