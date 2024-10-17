using Moq;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank;

namespace ExchangeRateUpdater.UnitTests
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_IgnoresCurrenciesNotProvidedByBankApi()
        {
            // Arrange
            var requestedCurrencies = new Currency[]
            {
                new("USD"), new("EUR"), new("RUB")
            };

            var exchangeRatesProvidedByBankApi = new BankApiExchangeRate[]
            {
                new("USD", 1.2m), new("EUR", 1.3m)
            };            

            var bankApiClientMock = new Mock<ICzechNationalBankApiClient>();
            bankApiClientMock.Setup(x => x.GetDailyExchangeRatesAsync())
                .ReturnsAsync(exchangeRatesProvidedByBankApi);

            var exchangeRateProvider = new ExchangeRateProvider(bankApiClientMock.Object);

            // Act
            var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(requestedCurrencies);

            // Assert
            Assert.Equal(2, actualExchangeRates.Count);

            var actualSourceCurrencyCodes = actualExchangeRates.Select(x => x.SourceCurrency.Code).ToHashSet();
            var expectedSourceCurrencyCodes = exchangeRatesProvidedByBankApi.Select(x => x.CurrencyCode).ToHashSet();
            Assert.Equal(expectedSourceCurrencyCodes, expectedSourceCurrencyCodes);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WhenCurrenciesEmpty_DoesNotCallBankApi()
        {
            // Arrange
            var requestedCurrencies = Array.Empty<Currency>();

            var bankApiClientMock = new Mock<ICzechNationalBankApiClient>();
            bankApiClientMock.Setup(x => x.GetDailyExchangeRatesAsync()).ReturnsAsync([]);

            var exchangeRateProvider = new ExchangeRateProvider(bankApiClientMock.Object);

            // Act
            var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(requestedCurrencies);

            // Assert
            Assert.Empty(actualExchangeRates);
            bankApiClientMock.Verify(x => x.GetDailyExchangeRatesAsync(), Times.Never);
        }
    }
}