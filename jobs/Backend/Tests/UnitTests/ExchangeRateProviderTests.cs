using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Entities;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Services;
using ExchangeRateUpdater.Infrastructure.Cnb;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace UnitTests;

public class ExchangeRateProviderTests
{
    [Fact]
    public async Task FilterOnlySupportedCurrencies()
    {
        // Arrange
        var exchangeRatesClientMock = new Mock<IExchangeRatesClient>();
        exchangeRatesClientMock
            .Setup(x => x.GetTodayExchangeRatesAsync())
            .ReturnsAsync(new ExchangeRates
            {
                Bank = "",
                Date = "",
                Position = "",
                Table = new ExchangeRateTable
                {
                    Type = "",
                    Rows = new[]
                    {
                        new ExchangeRateRow { Code = "CZK", Quantity = "1", ExchangeRate = "1" },
                        new ExchangeRateRow { Code = "EUR", Quantity = "1", ExchangeRate = "1" },
                        new ExchangeRateRow { Code = "USD", Quantity = "1", ExchangeRate = "1" },
                        new ExchangeRateRow { Code = "DKK", Quantity = "1", ExchangeRate = "1" }
                    }
                }
            });

        var supportedCurrencies = new[]
        {
            "SEK",
            "USD"
        };
        var supportedCurrenciesOptionsMock = new Mock<IOptions<SupportedCurrenciesOptions>>();
        supportedCurrenciesOptionsMock
            .Setup(x => x.Value)
            .Returns(new SupportedCurrenciesOptions
            {
                Currencies = supportedCurrencies
            });

        // Act
        var exchangeRateProvider = new ExchangeRateProvider(exchangeRatesClientMock.Object, supportedCurrenciesOptionsMock.Object);
        var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync();

        // Assert
        Assert.Equal(1, exchangeRates.Count);
        Assert.Equal("USD", exchangeRates.First().SourceCurrency.Code);
    }
}