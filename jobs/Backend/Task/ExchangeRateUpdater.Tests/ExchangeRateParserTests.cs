using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Tests
{  
    public class ExchangeRateParserTests
    {
        [Fact]
        public void ParseExchangeRates_ReturnsCorrectRates()
        {
            // Arrange
            var exchangeRateData = FileHelper.ReadTextFromFile("TestData.txt");

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            var parser = new ExchangeRateParser();

            // Act
            var rates = parser.ParseExchangeRates(exchangeRateData, new Currency("CZK"), currencies);

            // Assert
            Assert.Equal(2, rates.Count());

            var usdRate = rates.First(r => r.SourceCurrency.Code == "USD");
            Assert.Equal(24.000m, usdRate.Value);

            var eurRate = rates.First(r => r.SourceCurrency.Code == "EUR");
            Assert.Equal(26.000m, eurRate.Value);
        }
    }
}
