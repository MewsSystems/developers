using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;
using Moq;


namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {

        private Mock<ILogger<ExchangeRateProvider>> _loggerMock;
        public readonly Mock<IExchangeRateService> exchangeRateServiceMock;
        public ExchangeRateProvider exchangeRateProvider;

        public IEnumerable<ExchangeRate> testExchangeRatesData { get; set; }
        public IEnumerable<Currency> testCurrenciesData { get; set; }

        public ExchangeRateProviderTests() 
        {
            testExchangeRatesData = new List<ExchangeRate>()
            {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 0.89m),
            new ExchangeRate(new Currency("CZK"), new Currency("EUR"), 0.86m),
            new ExchangeRate(new Currency("CZK"), new Currency("AUD"), 1.37m),
            new ExchangeRate(new Currency("CZK"), new Currency("GBP"), 0.0075m),
            };
            testCurrenciesData = new List<Currency>(TestingData.currencies);

            _loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
            exchangeRateServiceMock = new Mock<IExchangeRateService>();
            exchangeRateServiceMock.Setup(s => s.GetExchangeRateAsync()).ReturnsAsync(testExchangeRatesData);
        }

        [Fact]
        public async Task GetExchangeRatesProvider_NotNullAsync()
        {
            exchangeRateProvider = new ExchangeRateProvider(_loggerMock.Object, exchangeRateServiceMock.Object);
            var result = await exchangeRateProvider.GetExchangeRatesAsync(testCurrenciesData);
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }
    }
}