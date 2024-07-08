using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_ShouldReturnExchangeRates_ForSpecifiedCurrencies()
        {
            // Arrange
            var mockExchangeRateService = new Mock<IExchangeRateService>();
            mockExchangeRateService
                .Setup(service => service.FetchExchangeRateDataAsync())
                .ReturnsAsync("31 Jan 2022 #21\nCountry|Currency|Amount|Code|Rate\nUSA|dollar|1|USD|21.657\nEurozone|euro|1|EUR|24.865\nUK|pound|1|GBP|29.752");

            var provider = new CnbExchangeRateProvider(mockExchangeRateService.Object);
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("GBP")
            };

            // Act
            var exchangeRates = await provider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Equal(3, exchangeRates.Count());
            Assert.Contains(exchangeRates, rate => rate.TargetCurrency.Code == "USD" && rate.Value == 21.657m);
            Assert.Contains(exchangeRates, rate => rate.TargetCurrency.Code == "EUR" && rate.Value == 24.865m);
            Assert.Contains(exchangeRates, rate => rate.TargetCurrency.Code == "GBP" && rate.Value == 29.752m);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldIgnoreNonSpecifiedCurrencies()
        {
            // Arrange
            var mockExchangeRateService = new Mock<IExchangeRateService>();
            mockExchangeRateService
                .Setup(service => service.FetchExchangeRateDataAsync())
                .ReturnsAsync("31 Jan 2022 #21\nCountry|Currency|Amount|Code|Rate\nUSA|dollar|1|USD|21.657\nEurozone|euro|1|EUR|24.865\nUK|pound|1|GBP|29.752");

            var provider = new CnbExchangeRateProvider(mockExchangeRateService.Object);
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // Act
            var exchangeRates = await provider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Equal(2, exchangeRates.Count());
            Assert.Contains(exchangeRates, rate => rate.TargetCurrency.Code == "USD" && rate.Value == 21.657m);
            Assert.Contains(exchangeRates, rate => rate.TargetCurrency.Code == "EUR" && rate.Value == 24.865m);
            Assert.DoesNotContain(exchangeRates, rate => rate.TargetCurrency.Code == "GBP");
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldReturnEmpty_ForNonExistingCurrencies()
        {
            // Arrange
            var mockExchangeRateService = new Mock<IExchangeRateService>();
            mockExchangeRateService
                .Setup(service => service.FetchExchangeRateDataAsync())
                .ReturnsAsync("31 Jan 2022 #21\nCountry|Currency|Amount|Code|Rate\nUSA|dollar|1|USD|21.657\nEurozone|euro|1|EUR|24.865\nUK|pound|1|GBP|29.752");

            var provider = new CnbExchangeRateProvider(mockExchangeRateService.Object);
            var currencies = new List<Currency>
            {
                new Currency("JPY"),
                new Currency("AUD")
            };

            // Act
            var exchangeRates = await provider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Empty(exchangeRates);
        }
    }
}
