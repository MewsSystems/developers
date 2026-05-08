using Castle.Core.Logging;
using ExchangeRateUpdater.Domain.DTO;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;
using Moq;


namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateServiceTests
    {

        public readonly Mock<IExchangeRateService> exchangeRateServiceMock;
        public ExchangeRatesDTO ExchangeRateDTOTestData { get; set; }  = new ExchangeRatesDTO();
        public IEnumerable<ExchangeRate> testExchangeRatesData { get; set; }
        public IEnumerable<ExchangeRateDTO> exchangeRatesDTO { get; set; }
        public IEnumerable<Currency> testCurrenciesData { get; set; }

        public ExchangeRateServiceTests() 
        {
            testExchangeRatesData = new List<ExchangeRate>()
            {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 0.89m),
            new ExchangeRate(new Currency("CZK"), new Currency("EUR"), 0.86m),
            new ExchangeRate(new Currency("CZK"), new Currency("AUD"), 1.37m),
            new ExchangeRate(new Currency("CZK"), new Currency("GBP"), 0.0075m),
            };

            exchangeRatesDTO = new List<ExchangeRateDTO>()
            {
            new ExchangeRateDTO { Amount = 100, CurrencyCode = "USD", Currency = "US Dollar", Country = "United States", Rate = 1.0m},
            new ExchangeRateDTO { Amount = 200, CurrencyCode = "EUR", Currency = "Euro", Country = "European Union", Rate = 0.89m },
            new ExchangeRateDTO { Amount = 300, CurrencyCode = "GBP", Currency = "British Pound", Country = "United Kingdom", Rate = 0.75m},
            new ExchangeRateDTO { Amount = 1000, CurrencyCode = "JPY", Currency = "Japanese Yen", Country = "Japan", Rate = 109.65m},
        };
            ExchangeRateDTOTestData.Rates = exchangeRatesDTO;
            testCurrenciesData = new List<Currency>(TestingData.currencies);

            exchangeRateServiceMock = new Mock<IExchangeRateService>();
            exchangeRateServiceMock.Setup(s => s.GetExchangeRateAsync()).ReturnsAsync(testExchangeRatesData);
        }


        [Fact]
        public async Task GetExchangeRates_NotNullAsync()
        {
            var result = await exchangeRateServiceMock.Object.GetExchangeRateAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }
        [Fact]

        public async Task ConvertsExchangeRates_OkAsync()
        {
            var result = ConverterExtension.ToExchangeRates(ExchangeRateDTOTestData);

            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }
    }
}