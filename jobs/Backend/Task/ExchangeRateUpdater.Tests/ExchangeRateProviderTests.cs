using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRates_ReturnsCorrectRates()
        {
            // Arrange
            var exchangeRateData = FileHelper.ReadTextFromFile("TestData.txt");

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR")
            };
            var targetCurrency = new Currency("CZK");

            var fetcherMock = new Mock<IExchangeRateFetcher>();
            fetcherMock.Setup(f => f.FetchExchangeRateData()).ReturnsAsync(exchangeRateData);

            var parserMock = new Mock<IExchangeRateParser>();
            parserMock.Setup(p => p.ParseExchangeRates(exchangeRateData, targetCurrency, currencies)).Returns(new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), targetCurrency, 24.000m),
                new ExchangeRate(new Currency("EUR"), targetCurrency, 26.000m)
            });

            var provider = new ExchangeRateProvider(fetcherMock.Object, parserMock.Object);

            // Act
            var rates = await provider.GetExchangeRates(currencies);

            // Assert
            Assert.Equal(2, rates.Count());

            var usdRate = rates.First(r => r.SourceCurrency.Code == "USD");
            Assert.Equal(24.000m, usdRate.Value);

            var eurRate = rates.First(r => r.SourceCurrency.Code == "EUR");
            Assert.Equal(26.000m, eurRate.Value);

            fetcherMock.Verify(f => f.FetchExchangeRateData(), Times.Once);
            parserMock.Verify(p => p.ParseExchangeRates(exchangeRateData, targetCurrency, currencies), Times.Once);
        }
    }
}
