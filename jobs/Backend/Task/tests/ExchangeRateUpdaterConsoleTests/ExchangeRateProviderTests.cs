using CNB.ApiClient;
using CNB.ApiClient.Models;
using ExchangeRateUpdater.Console;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Shared.Caching;
using Moq;

namespace ExchangeRateUpdaterConsoleTests;

public class ExchangeRateProviderTests
{
    private readonly ExchangeRateProvider _sut;
    private readonly Mock<ICNBApiClient> _clientMock;
    private readonly Mock<ICache> _cacheMock;
    private readonly ExratesDailyResponse _exratesDailyResponse;

    public ExchangeRateProviderTests()
    {
        _clientMock = new();
        _cacheMock = new();

        _sut = new(_clientMock.Object, _cacheMock.Object);

        _exratesDailyResponse = new ExratesDailyResponse
        {
            Rates =
            [
                new ExrateApiModel
                {
                    Amount = 1,
                    Country = "EMU",
                    Currency = "euro",
                    CurrencyCode = "EUR",
                    Rate = 25.15,
                    Order = 54,
                    ValidFor = DateTime.Parse("2024-03-15")
                }
            ]
        };
    }

    [Fact]
    public async Task GetExchangeRates_ReturnsExratesFromApiAndSavesToCache_WhenCacheEmpty()
    {
        //Arrange
        var currencies = new List<Currency> { new("EUR") };

        _clientMock
            .Setup(x => x.GetDailyExrates(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_exratesDailyResponse);
        _cacheMock
            .Setup(x => x.GetData<ExratesDailyResponse>(It.IsAny<string>()))
            .Returns(default(ExratesDailyResponse));

        //Act
        var result = await _sut.GetExchangeRates(currencies);

        //Assert
        var exrate = result.First();
        Assert.Equal("CZK", exrate.SourceCurrency.Code);
        Assert.Equal("EUR", exrate.TargetCurrency.Code);
        Assert.Equal(25.15M, exrate.Value);
        _cacheMock.Verify(
            x => x.SetData("ExratesDailyResponse", _exratesDailyResponse),
            Times.Once
        );
    }

    [Fact]
    public async Task GetExchangeRates_ReturnsExratesFromCache_WhenCacheFilled()
    {
        //Arrange
        var currencies = new List<Currency> { new("EUR") };
        var apiExrate = new ExrateApiModel
        {
            Amount = 1,
            Country = "EMU",
            Currency = "euro",
            CurrencyCode = "EUR",
            Rate = 25.15,
            Order = 54,
            ValidFor = DateTime.Parse("2024-03-15")
        };

        _cacheMock
            .Setup(x => x.GetData<ExratesDailyResponse>(It.IsAny<string>()))
            .Returns(new ExratesDailyResponse { Rates = [apiExrate] });

        //Act
        var result = await _sut.GetExchangeRates(currencies);

        //Assert
        var exrate = result.First();
        Assert.Equal("CZK", exrate.SourceCurrency.Code);
        Assert.Equal("EUR", exrate.TargetCurrency.Code);
        Assert.Equal(25.15M, exrate.Value);
    }

    [Fact]
    public async Task GetExchangeRates_ReturnsEmptyList_WhenIncorrectCurrency()
    {
        //Arrange
        var currencies = new List<Currency> { new("XYZ") };

        _clientMock
            .Setup(x => x.GetDailyExrates(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_exratesDailyResponse);
        _cacheMock
            .Setup(x => x.GetData<ExratesDailyResponse>(It.IsAny<string>()))
            .Returns(default(ExratesDailyResponse));

        //Act
        var result = await _sut.GetExchangeRates(currencies);

        //Assert
        Assert.Empty(result);
    }
}
