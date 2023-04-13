using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class CzechExchangeRateProviderTests
    {
        private readonly Mock<IExchangeRateFetcher> _fetcherMock;
        private readonly CzechExchangeRateParser _parser;
        private readonly MockClock _mockClock;

        public CzechExchangeRateProviderTests()
        {
            var dailyExchangeRateData = FileHelper.ReadTextFromFile("Files/DailyTestData.txt");
            var monthlyExchangeRateData = FileHelper.ReadTextFromFile("Files/MonthlyTestData.txt");

            _fetcherMock = new Mock<IExchangeRateFetcher>();
            _fetcherMock.Setup(f => f.FetchDailyExchangeRateData(It.IsAny<DateOnly?>())).ReturnsAsync(dailyExchangeRateData);
            _fetcherMock.Setup(f => f.FetchMonthlyExchangeRateData(It.IsAny<DateOnly?>())).ReturnsAsync(monthlyExchangeRateData);

            _parser = new CzechExchangeRateParser();
            _mockClock = new MockClock(DateOnly.Parse("2023-04-12"));
        }

        [Fact]
        public async Task GetExchangeRates_ReturnsCorrectRates()
        {
            // Arrange
            var currencies = new[]
            {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("TND")
        };
            var provider = new CzechExchangeRateProvider(_fetcherMock.Object, _parser, _mockClock);

            // Act
            var rates = await provider.GetExchangeRates(currencies);

            // Assert
            Assert.Equal(3, rates.Count());

            var usdRate = rates.First(r => r.SourceCurrency.Code == "USD");
            Assert.Equal(24.000m, usdRate.Value);

            var eurRate = rates.First(r => r.SourceCurrency.Code == "EUR");
            Assert.Equal(26.000m, eurRate.Value);

            var tndRate = rates.First(r => r.SourceCurrency.Code == "TND");
            Assert.Equal(7.081m, tndRate.Value);
        }

        [Fact]
        public async Task GetExchangeRates_InvalidCurrency_ReturnsEmptyList()
        {
            // Arrange
            var provider = new CzechExchangeRateProvider(_fetcherMock.Object, _parser, _mockClock);
            var currencies = new List<Currency> { new Currency("INVALID") };

            // Act
            var exchangeRates = await provider.GetExchangeRates(currencies);

            // Assert
            Assert.Empty(exchangeRates);
        }
    }
}
