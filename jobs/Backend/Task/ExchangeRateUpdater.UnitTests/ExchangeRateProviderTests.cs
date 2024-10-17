using Moq;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.UnitTests
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_IgnoresCurrenciesNotProvidedByApi()
        {
            // Arrange
            var targetCurrencyCode = "CZK";
            var requestedCurrencies = new Currency[]
            {
                new("USD"), new("EUR"), new("RUB")
            };
            var exchangeRatesProvidedByApi = new ApiExchangeRate[]
            {
                new("USD", 1.2m), new("EUR", 1.3m)
            };
            var apiClientFactoryMock = CreateExchangeApiClientFactoryMock(exchangeRatesProvidedByApi, targetCurrencyCode);               

            var exchangeRateProvider = new ExchangeRateProvider(apiClientFactoryMock.Object);

            // Act
            var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(requestedCurrencies, targetCurrencyCode);

            // Assert
            Assert.Equal(2, actualExchangeRates.Count);

            var actualSourceCurrencyCodes = actualExchangeRates.Select(x => x.SourceCurrency.Code).ToHashSet();
            var expectedSourceCurrencyCodes = new[] { "USD", "EUR" }.ToHashSet();
            Assert.Equal(expectedSourceCurrencyCodes, expectedSourceCurrencyCodes);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WhenCurrenciesEmpty_DoesNotCallExchangeRateApi()
        {
            // Arrange
            var targetCurrencyCode = "CZK";
            var requestedCurrencies = Array.Empty<Currency>();
            var apiClientFactoryMock = CreateExchangeApiClientFactoryMock([], targetCurrencyCode);           

            var exchangeRateProvider = new ExchangeRateProvider(apiClientFactoryMock.Object);

            // Act
            var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(requestedCurrencies, targetCurrencyCode);

            // Assert
            Assert.Empty(actualExchangeRates);
            apiClientFactoryMock.Verify(x => x.CreateExchangeRateApiClient(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsDataOnlyForRequestedCurrencies()
        {
            // Arrange
            var targetCurrencyCode = "CZK";
            var requestedCurrencies = new Currency[]
            {
                new("USD"), new("EUR"), new("RUB")
            };
            var exchangeRatesProvidedByApi = new ApiExchangeRate[]
            {
                new("USD", 1.2m), new("EUR", 1.3m), new("KES", 1.2m), new("RUB", 1.3m), new("HBC", 1.2m)
            };
            var apiClientFactoryMock = CreateExchangeApiClientFactoryMock(exchangeRatesProvidedByApi, targetCurrencyCode);           

            var exchangeRateProvider = new ExchangeRateProvider(apiClientFactoryMock.Object);

            // Act
            var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(requestedCurrencies, targetCurrencyCode);

            // Assert
            Assert.Equal(3, actualExchangeRates.Count);

            var actualSourceCurrencyCodes = actualExchangeRates.Select(x => x.SourceCurrency.Code).ToHashSet();
            var expectedSourceCurrencyCodes = new[] { "USD", "EUR", "RUB" }.ToHashSet();
            Assert.Equal(expectedSourceCurrencyCodes, expectedSourceCurrencyCodes);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsCorrectTargetCurrency()
        {
            // Arrange
            var targetCurrencyCode = "CZK";
            var requestedCurrencies = new Currency[]
            {
                new("USD"), new("EUR"), new("RUB")
            };
            var exchangeRatesProvidedByApi = new ApiExchangeRate[]
            {
                new("USD", 1.2m), new("EUR", 1.3m), new("RUB", 1.3m)
            };
            var apiClientFactoryMock = CreateExchangeApiClientFactoryMock(exchangeRatesProvidedByApi, targetCurrencyCode);            

            var exchangeRateProvider = new ExchangeRateProvider(apiClientFactoryMock.Object);

            // Act
            var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(requestedCurrencies, targetCurrencyCode);

            // Assert
            Assert.NotEmpty(actualExchangeRates);
            Assert.All(actualExchangeRates, rate => Assert.Equal(targetCurrencyCode, rate.TargetCurrency.Code));
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsCorrectRateValues()
        {
            // Arrange
            var targetCurrencyCode = "CZK";
            var requestedCurrencies = new Currency[]
            {
                new("USD"), new("EUR"), new("RUB")
            };
            var exchangeRatesProvidedByApi = new ApiExchangeRate[]
            {
                new("USD", 0.2m), new("EUR", 1.8m), new("RUB", 5.8m)
            };
            var apiClientFactoryMock = CreateExchangeApiClientFactoryMock(exchangeRatesProvidedByApi, targetCurrencyCode);           

            var exchangeRateProvider = new ExchangeRateProvider(apiClientFactoryMock.Object);

            // Act
            var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(requestedCurrencies, targetCurrencyCode);

            // Assert
            Assert.Equal(3, actualExchangeRates.Count);
            Assert.All(exchangeRatesProvidedByApi, apiRate => 
            {
                var actualRate = actualExchangeRates.FirstOrDefault(x => x.SourceCurrency.Code == apiRate.CurrencyCode);
                Assert.Equal(apiRate.Rate, actualRate?.Value);
            });                     
        }

        private static Mock<IExchangeRateApiClientFactory> CreateExchangeApiClientFactoryMock(
            ApiExchangeRate[] exchangeRates, string targetCurrencyCode)
        {
            var exchangeRateApiClientMock = new Mock<IExchangeRateApiClient>();
            exchangeRateApiClientMock.Setup(x => x.GetDailyExchangeRatesAsync())
                .ReturnsAsync(exchangeRates);

            var apiClientFactoryMock = new Mock<IExchangeRateApiClientFactory>();
            apiClientFactoryMock.Setup(x => x.CreateExchangeRateApiClient(targetCurrencyCode))
                .Returns(exchangeRateApiClientMock.Object);

            return apiClientFactoryMock;
        }
    }
}