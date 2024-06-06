using ExchangeRateUpdater.ExchangeRate.Constant;
using ExchangeRateUpdater.ExchangeRate.Exception;
using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank;
using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdaterTests.ExchangeRate.Provider.CzechNationalBank
{
    public class CzechNationalBankExchangeRateProviderTests
    {
        [Fact]
        public async Task GetDailyExchangeRates_ReturnsCorrectExchangeRates()
        {
            // Arrange
            var targetDate = new DateOnly(2024, 6, 1);
            var language = Language.EN;
            var cancellationToken = new CancellationToken();

            var expectedCzechNationalBankResponse = new CzechNationalBankDailyExchangeRateResponse
            {
                ExchangeRates =
                [
                    new CzechNationalBankExchangeRate { Currency = "US Dollar", CurrencyCode = "USD", Country = "United States", Rate = 25, Amount = 1 },
                    new CzechNationalBankExchangeRate { Currency = "Euro", CurrencyCode = "EUR", Country = "European Union", Rate = 30, Amount = 1 }
                ]
            };

            var czechNationalBankClientMock = new Mock<ICzechNationalBankClient>();
            czechNationalBankClientMock.Setup(c => c.GetDailyExchangeRates(It.IsAny<FetchCzechNationalBankDailyExchangeRateRequest>(), cancellationToken))
                                       .ReturnsAsync(expectedCzechNationalBankResponse);

            var loggerMock = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>();

            var exchangeRateProvider = new CzechNationalBankExchangeRateProvider(czechNationalBankClientMock.Object, loggerMock.Object);

            // Act
            var exchangeRates = await exchangeRateProvider.GetDailyExchangeRates(targetDate, language, cancellationToken);

            // Assert
            Assert.NotNull(exchangeRates);
            Assert.Equal(expectedCzechNationalBankResponse.ExchangeRates.Count(), exchangeRates.Count());

            // Verify that the exchange rates are correctly mapped
            foreach (var expectedExchangeRate in expectedCzechNationalBankResponse.ExchangeRates)
            {
                var actualExchangeRate = exchangeRates.FirstOrDefault(e => e.Currency == expectedExchangeRate.Currency);
                Assert.NotNull(actualExchangeRate);
                Assert.Equal(expectedExchangeRate.CurrencyCode, actualExchangeRate.CurrencyCode);
                Assert.Equal(expectedExchangeRate.Country, actualExchangeRate.Country);
                Assert.Equal(expectedExchangeRate.Rate / expectedExchangeRate.Amount, actualExchangeRate.Rate);
            }
        }

        [Fact]
        public void GetSupportedLanguages_ReturnsSupportedLanguages()
        {
            // Arrange
            var czechNationalBankClientMock = new Mock<ICzechNationalBankClient>();
            var loggerMock = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>();

            var exchangeRateProvider = new CzechNationalBankExchangeRateProvider(czechNationalBankClientMock.Object, loggerMock.Object);

            // Act
            var supportedLanguages = exchangeRateProvider.GetSupportedLanguages();

            // Assert
            Assert.NotNull(supportedLanguages);
            Assert.Contains(Language.CZ, supportedLanguages);
            Assert.Contains(Language.EN, supportedLanguages);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ThrowsExchangeRateUpdaterException_WhenClientThrowsException()
        {
            // Arrange
            var targetDate = new DateOnly(2024, 6, 1);
            var language = Language.EN;
            var cancellationToken = new CancellationToken();

            var czechNationalBankClientMock = new Mock<ICzechNationalBankClient>();
            czechNationalBankClientMock.Setup(c => c.GetDailyExchangeRates(It.IsAny<FetchCzechNationalBankDailyExchangeRateRequest>(), cancellationToken))
                                       .ThrowsAsync(new HttpRequestException("Error"));

            var loggerMock = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>();

            var exchangeRateProvider = new CzechNationalBankExchangeRateProvider(czechNationalBankClientMock.Object, loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ExchangeRateUpdaterException>(() =>
                exchangeRateProvider.GetDailyExchangeRates(targetDate, language, cancellationToken));
        }
    }
}
