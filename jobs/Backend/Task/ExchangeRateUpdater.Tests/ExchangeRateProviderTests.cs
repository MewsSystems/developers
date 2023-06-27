using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    [Fact]
    public void GetExchangeRates_NoCurrencies_ReturnsEmptyList()
    {
        // Arrange
        var gatewayMock = new Mock<ICzechNationalBankExchangeRateGateway>();
        var provider = new ExchangeRateProvider(gatewayMock.Object);
        gatewayMock.Setup(x => x.GetCurrentRates()).Returns(new CnbExchangeRates());

        // Act
        var result = provider.GetExchangeRates(Enumerable.Empty<Currency>());

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetExchangeRates_ReturnsExpectedExchangeRate()
    {
        // Arrange
        var gatewayMock = new Mock<ICzechNationalBankExchangeRateGateway>();
        var provider = new ExchangeRateProvider(gatewayMock.Object);
        var czkCurrency = new Currency("CZK");
        var usdCurrency = new Currency("USD");
        var currencies = new List<Currency> { usdCurrency };
        var expectedExchangeRate = new ExchangeRate(czkCurrency, usdCurrency, 1.550m);
        var gatewayResponse = new CnbExchangeRates
        {
            Rates = new List<CnbExchangeRate>
            {
                new()
                {
                    Rate = 1.550m,
                    CurrencyCode = "USD",
                }
            }
        };
        gatewayMock.Setup(x => x.GetCurrentRates()).Returns(gatewayResponse);

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
        var gatewayMock = new Mock<ICzechNationalBankExchangeRateGateway>();
        var provider = new ExchangeRateProvider(gatewayMock.Object);
        var usdCurrency = new Currency("USD");
        var currencies = new List<Currency> { usdCurrency };
        var gatewayResponse = new CnbExchangeRates
        {
            Rates = new List<CnbExchangeRate>
            {
                new()
                {
                    Rate = 1.550m,
                    CurrencyCode = "USD",
                },
                new()
                {
                    Rate = 1.550m,
                    CurrencyCode = "NZD",
                }
            }
        };
        gatewayMock.Setup(x => x.GetCurrentRates()).Returns(gatewayResponse);

        // Act
        var result = provider.GetExchangeRates(currencies);

        // Assert
        result.Should().OnlyContain(x => x.TargetCurrency.Code == usdCurrency.Code);
    }

    [Fact]
    public void GetExchangeRates_CurrencyMissingInSource_ReturnsNoExchangeRate()
    {
        // Arrange
        var gatewayMock = new Mock<ICzechNationalBankExchangeRateGateway>();
        var provider = new ExchangeRateProvider(gatewayMock.Object);
        var madeUpCurrency = new Currency("XXX");
        var currencies = new List<Currency> { madeUpCurrency };
        var gatewayResponse = new CnbExchangeRates
        {
            Rates = new List<CnbExchangeRate>
            {
                new()
                {
                    Rate = 1.550m,
                    CurrencyCode = "USD",
                },
                new()
                {
                    Rate = 1.550m,
                    CurrencyCode = "NZD",
                }
            }
        };
        gatewayMock.Setup(x => x.GetCurrentRates()).Returns(gatewayResponse);
        
        // Act
        var result = provider.GetExchangeRates(currencies);

        // Assert
        result.Should().BeEmpty();
    }
}