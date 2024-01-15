using ExchangeRateUpdater.CnbExchangeRateProvider;
using ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient;
using ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient.Models;
using ExchangeRateUpdater.ExchangeRateProvider;

namespace ExchangeRateUpdaterTests;

public class CnbExchangeRateProviderTests
{
    private readonly Mock<IApiClient> _apiClientMock = new();

    private static readonly IEnumerable<Currency> Currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("INR"),
        new Currency("XYZ")
    };

    [Fact]
    public async Task GetExchangeRates_WhenApiResultIsEmpty_ReturnsNoExchangeRates()
    {
        // Arrange
        _apiClientMock.Setup(x => x.GetDailyExchangeRatesAsync())
            .ReturnsAsync(new DailyExchangeRateApiModel());

        var exchangeRateProvider = new CnbExchangeRateProvider(_apiClientMock.Object);
        
        // Act
        var result = await exchangeRateProvider.GetExchangeRatesAsync(Currencies);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRates_WhenApiResultIsNull_ReturnsNoExchangeRates()
    {
        // Arrange
        _apiClientMock.Setup(x => x.GetDailyExchangeRatesAsync())
            .ReturnsAsync((DailyExchangeRateApiModel?)null);

        var exchangeRateProvider = new CnbExchangeRateProvider(_apiClientMock.Object);
        
        // Act
        var result = await exchangeRateProvider.GetExchangeRatesAsync(Currencies);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRates_WhenApiResultDoesNotContainRequestedCurrencies_ReturnsNoExchangeRates()
    {
        // Arrange
        _apiClientMock.Setup(x => x.GetDailyExchangeRatesAsync())
            .ReturnsAsync(new DailyExchangeRateApiModel
            {
                Rates = new List<ExchangeRateApiModel>
                {
                    new() { CurrencyCode = "AUD", Rate = 10.3m, Amount = 1 },
                    new() { CurrencyCode = "AMD", Rate = 0.1m, Amount = 1 },
                }
            });

        var exchangeRateProvider = new CnbExchangeRateProvider(_apiClientMock.Object);
        
        // Act
        var result = await exchangeRateProvider.GetExchangeRatesAsync(Currencies);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRates_WhenNoCurrenciesProvided_ReturnsNoExchangeRates()
    {
        // Arrange
        _apiClientMock.Setup(x => x.GetDailyExchangeRatesAsync())
            .ReturnsAsync(new DailyExchangeRateApiModel
            {
                Rates = new List<ExchangeRateApiModel>
                {
                    new() { CurrencyCode = "AUD", Rate = 10.3m, Amount = 1 },
                    new() { CurrencyCode = "AMD", Rate = 0.1m, Amount = 1 },
                }
            });

        var exchangeRateProvider = new CnbExchangeRateProvider(_apiClientMock.Object);
        
        // Act
        var result = await exchangeRateProvider.GetExchangeRatesAsync(Enumerable.Empty<Currency>());

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRates_WhenApiReturnsCorrectRates_ReturnsExpectedExchangeRates()
    {
        // Arrange
        _apiClientMock.Setup(x => x.GetDailyExchangeRatesAsync())
            .ReturnsAsync(new DailyExchangeRateApiModel
            {
                Rates = new List<ExchangeRateApiModel>
                {
                    new() { CurrencyCode = "USD", Rate = 20.3m, Amount = 1 },
                    new() { CurrencyCode = "EUR", Rate = 24, Amount = 1 },
                    new() { CurrencyCode = "JPY", Rate = 13.04m, Amount = 1 },
                    new() { CurrencyCode = "INR", Rate = 27.199m, Amount = 100 },
                }
            });

        var exchangeRateProvider = new CnbExchangeRateProvider(_apiClientMock.Object);

        // Act
        var result = (await exchangeRateProvider.GetExchangeRatesAsync(Currencies)).ToArray();

        // Assert
        Assert.Equal(4, result.Length);
        Assert.Equal("EUR/CZK=24", result[0].ToString());
        Assert.Equal("INR/CZK=0.27199", result[1].ToString());
        Assert.Equal("JPY/CZK=13.04", result[2].ToString());
        Assert.Equal("USD/CZK=20.3", result[3].ToString());
    }
}