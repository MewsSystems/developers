using ExchangeRateUpdater.Infrastructure.ExchangeRates;
using ExchangeRateUpdater.Model.ExchangeRates;
using ExchangeRateUpdater.Services.ExchangeRates;
using Moq;

namespace ExchangeRateUpdater.Services.Tests.ExchangeRates;

[TestClass]
public class ExchangeProviderTests
{
    [TestMethod]
    public async Task ExchangeRateProvider_GetExchangeRatesAsync_ReturnsSomeExchangeRates()
    {
        // Arrange
        List<Currency> currencies =
        [
            new("CZK"),
            new("EUR"),
            new("USD")
        ];

        var czkUsdExchangeRate = new ExchangeRate(new("CZK"), new("USD"), 22.0m);
        var usdCzkExchangeRate = new ExchangeRate(new("USD"), new("CZK"), 0.045m);
        var czkGbpExchangeRate = new ExchangeRate(new("CZK"), new("GBP"), 28.0m);
        var eurUsdExchangeRate = new ExchangeRate(new("EUR"), new("USD"), 0.9m);
        var mxnCadExchangeRate = new ExchangeRate(new("MXN"), new("CAD"), 0.08m);
        
        List<ExchangeRate> sourceExchangeRates =
        [
            czkUsdExchangeRate,
            usdCzkExchangeRate,
            czkGbpExchangeRate,
            eurUsdExchangeRate,
            mxnCadExchangeRate
        ];
        
        var exchangeRateProvider = CreateExchangeRateProviderFixture(sourceExchangeRates);

        // Act
        var exchangeRates = (await exchangeRateProvider.GetExchangeRatesAsync(currencies, CancellationToken.None)).ToList();

        // Assert
        Assert.IsNotNull(exchangeRates);
        
        CollectionAssert.Contains(exchangeRates, czkUsdExchangeRate);
        CollectionAssert.Contains(exchangeRates, usdCzkExchangeRate);
        CollectionAssert.Contains(exchangeRates, eurUsdExchangeRate);
    }
    
    [TestMethod]
    public async Task ExchangeRateProvider_GetExchangeRatesAsync_ReturnsNoExchangeRates()
    {
        // Arrange
        var exchangeRateProvider = CreateExchangeRateProviderFixture([]);
        
        // Act
        var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync([], CancellationToken.None);
        
        // Assert
        Assert.IsNotNull(exchangeRates);
        Assert.IsTrue(!exchangeRates.Any());
    }

    private static ExchangeRateProvider CreateExchangeRateProviderFixture(IEnumerable<ExchangeRate> exchangeRates)
    {
        var exchangeRateDataSourceMock = new Mock<IExchangeRateDataSource>();

        exchangeRateDataSourceMock
            .Setup(dataSource => dataSource.GetExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(exchangeRates));

        return new ExchangeRateProvider(exchangeRateDataSourceMock.Object);
    }
}