using FluentAssertions;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    [Fact]
    public void GetExchangeRates_NoCurrencies_ReturnsEmptyList()
    {
        // Arrange
        var provider = new ExchangeRateProvider(new CzechNationalBankExchangeRateGateway());

        // Act
        var result = provider.GetExchangeRates(Enumerable.Empty<Currency>());

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetExchangeRates_ReturnsExpectedExchangeRate()
    {
        // Arrange
        var provider = new ExchangeRateProvider(new CzechNationalBankExchangeRateGateway());
        var czkCurrency = new Currency("CZK");
        var usdCurrency = new Currency("USD");
        var currencies = new List<Currency> { usdCurrency };
        var expectedExchangeRate = new ExchangeRate(czkCurrency, usdCurrency, 21.550m);

        // Act
        var result = provider.GetExchangeRates(currencies);

        // Assert
        var exchangeRate = result.Single();
        exchangeRate.SourceCurrency.Code.Should().Be(expectedExchangeRate.SourceCurrency.Code);
        exchangeRate.TargetCurrency.Code.Should().Be(expectedExchangeRate.TargetCurrency.Code);
        exchangeRate.Value.Should().Be(expectedExchangeRate.Value);
    }
    
    [Fact]
    public void GetExchangeRates_ForSpecificCurrency_OnlyReturnsCurrencyExchangeRates()
    {
        // Arrange
        var provider = new ExchangeRateProvider(new CzechNationalBankExchangeRateGateway());
        var czkCurrency = new Currency("CZK");
        var usdCurrency = new Currency("USD");
        var currencies = new List<Currency> { usdCurrency };

        // Act
        var result = provider.GetExchangeRates(currencies);

        // Assert
        result.Should().OnlyContain(x => x.SourceCurrency.Code == czkCurrency.Code && x.TargetCurrency.Code == usdCurrency.Code);
    }
    
    [Fact]
    public void GetExchangeRates_CurrencyMissingInSource_ReturnsNoExchangeRate()
    {
        // Arrange
        var provider = new ExchangeRateProvider(new CzechNationalBankExchangeRateGateway());
        var madeUpCurrency = new Currency("XXX");
        var currencies = new List<Currency> { madeUpCurrency };

        // Act
        var result = provider.GetExchangeRates(currencies);

        // Assert
        result.Should().BeEmpty();
    }
}