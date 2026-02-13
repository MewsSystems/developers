using ExchangeRateUpdater.Business;
using ExchangeRateUpdater.Business.Models;
using ExchangeRateUpdater.Business.Services;
using Moq;

namespace ExchangeRateUpdaterUnitTest
{
    public class ExchangeRateProviderTest
    {
        private readonly IExchangeRateProvider __ExchangeRateProvider;
        private readonly Mock<IExchangeRateService> __ExchangeRateServiceMock;

        public ExchangeRateProviderTest()
        {
            __ExchangeRateServiceMock = new Mock<IExchangeRateService>();
            __ExchangeRateProvider = new ExchangeRateProvider(__ExchangeRateServiceMock.Object);
        }

        [Fact]
        public async Task ExchangeRateProvider_GetExchangeRatesAsync_ShouldReturnResultsAsync()
        {
            List<ExchangeRateModel> _ExchangeRateModels =
            [
                new ExchangeRateModel() { CurrencyCode = "USD", Rate = 20.43m },
                new ExchangeRateModel() { CurrencyCode = "AUD", Rate = 14.48m },
                new ExchangeRateModel() { CurrencyCode = "BRL", Rate = 3.92m },
            ];

            __ExchangeRateServiceMock
                .Setup(mock => mock.GetExchangeRatesAsync())
                .Returns(Task.FromResult(_ExchangeRateModels))
                .Verifiable();

            List<Currency> _Currencies = 
            [
                new Currency("AUD"),
                new Currency("USD")
            ];

            List<ExchangeRate> _Results = await __ExchangeRateProvider.GetExchangeRatesAsync(_Currencies);

            Assert.NotNull(_Results);
            Assert.Equal(2, _Results.Count);
        }
    }
}
