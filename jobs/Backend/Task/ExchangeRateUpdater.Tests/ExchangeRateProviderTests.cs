using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Clients.Models;
using ExchangeRateUpdater.Domain;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    [Fact]
    public void GetExchangeRates_NoCurrencies_ReturnsEmptyList()
    {
        // Arrange
        var clientMock = new Mock<ICzechNationalBankExchangeRateClient>();
        var provider = new ExchangeRateProvider(clientMock.Object);
        clientMock.Setup(x => x.GetCurrentRates()).Returns(new CnbExchangeRates());

        // Act
        var result = provider.GetExchangeRates(Enumerable.Empty<Currency>());

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetExchangeRates_ReturnsExpectedExchangeRate()
    {
        // Arrange
        var clientMock = new Mock<ICzechNationalBankExchangeRateClient>();
        var provider = new ExchangeRateProvider(clientMock.Object);
        var czkCurrency = new Currency("CZK");
        var usdCurrency = new Currency("USD");
        var currencies = new List<Currency> { usdCurrency };
        var expectedExchangeRate = new ExchangeRate(czkCurrency, usdCurrency, 1.550m);
        var cnbResponse = new CnbExchangeRates
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
        clientMock.Setup(x => x.GetCurrentRates()).Returns(cnbResponse);

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
        var clientMock = new Mock<ICzechNationalBankExchangeRateClient>();
        var provider = new ExchangeRateProvider(clientMock.Object);
        var usdCurrency = new Currency("USD");
        var currencies = new List<Currency> { usdCurrency };
        var cnbResponse = new CnbExchangeRates
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
        clientMock.Setup(x => x.GetCurrentRates()).Returns(cnbResponse);

        // Act
        var result = provider.GetExchangeRates(currencies);

        // Assert
        result.Should().OnlyContain(x => x.TargetCurrency.Code == usdCurrency.Code);
    }

    [Fact]
    public void GetExchangeRates_CurrencyMissingInSource_ReturnsNoExchangeRate()
    {
        // Arrange
        var clientMock = new Mock<ICzechNationalBankExchangeRateClient>();
        var provider = new ExchangeRateProvider(clientMock.Object);
        var madeUpCurrency = new Currency("XXX");
        var currencies = new List<Currency> { madeUpCurrency };
        var cnbResponse = new CnbExchangeRates
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
        clientMock.Setup(x => x.GetCurrentRates()).Returns(cnbResponse);
        
        // Act
        var result = provider.GetExchangeRates(currencies);

        // Assert
        result.Should().BeEmpty();
    }
}