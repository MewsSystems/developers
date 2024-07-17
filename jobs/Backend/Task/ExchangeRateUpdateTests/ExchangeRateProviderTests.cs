using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdateTests;

public class ExchangeRateProviderTests
{
    [Fact]
    public async Task CanGetExchangeRates()
    {
        // Arrange
        Dictionary<string, DailyExRateItem> fakeExchangeRates = new()
        {
            { "THB", new(1, "Thailand", "baht", "THB", 136, 64.486m, DateTime.Today) },
            { "TRY", new(1, "Turkey", "lira", "TRY", 136, 70.298m, DateTime.Today) },
            { "JPY", new(1, "Japan", "yen", "JPY", 136, 14.689m, DateTime.Today) },
            { "EUR", new(1, "EMU", "euro", "CAD", 136, 25.36m, DateTime.Today) },
            { "USD", new(1, "USA", "dollar", "USD", 136, 23.264m, DateTime.Today) },
        };
        Mock<ICnbApiClient> mockCnbApiClient = new();
        mockCnbApiClient
            .Setup(x => x.GetExchangeRatesFromCnbApi(It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeExchangeRates);

        Mock<ILogger<ExchangeRateProvider>> mockLogger = new();
        Mock<IHost> mockHost = new();

        ExchangeRateProvider exchangeRateProvider = new(mockLogger.Object, mockCnbApiClient.Object, mockHost.Object);

        // Act
        var exchangeRates = (await exchangeRateProvider.GetExchangeRates(CancellationToken.None)).ToList();

        // Assert

        // Even though there are more than 5 currencies that this method is looking for, only 5 are provided by the mocked API. Therefore, this should never be more than 5 because unknown exchange rates are omitted.
        Assert.Equal(5, exchangeRates.Count());

        foreach (var fakeExchangeRate in fakeExchangeRates)
        {
            int removed = exchangeRates.RemoveAll(er => er.SourceCurrency.Code == fakeExchangeRate.Key && er.Value == fakeExchangeRate.Value.Rate);
            // There should only be one exchange rate per currency in the list.
            Assert.Equal(1, removed);
        }
    }
}
