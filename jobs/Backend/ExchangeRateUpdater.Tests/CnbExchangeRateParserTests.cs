using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Tests
{
    public class CnbExchangeRateParserTests
    {
        [Fact]
        public void Parse_ReturnsExpectedExchangeRates()
        {
            // Arrange
            var jsonResponse = new ExchangeRateApiResponse
            {
                Rates = new List<RateDTO>
                {
                    new RateDTO { CurrencyCode = "USD", RateValue = 23.341m, Amount = 1 },
                    new RateDTO { CurrencyCode = "EUR", RateValue = 24.945m, Amount = 1 },
                    new RateDTO { CurrencyCode = "GBP", RateValue = 29.515m, Amount = 1 }
                }
            };
            var jsonData = JsonConvert.SerializeObject(jsonResponse);

            var parser = new CnbExchangeRateParser();
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // Act
            var exchangeRates = parser.Parse(jsonData, currencies);

            // Assert
            Assert.Equal(2, exchangeRates.Count());
            Assert.Contains(exchangeRates, r => r.TargetCurrency.Code == "USD" && r.Value == 23.341m);
            Assert.Contains(exchangeRates, r => r.TargetCurrency.Code == "EUR" && r.Value == 24.945m);
        }

        [Fact]
        public void Parse_EmptyString_ReturnsEmptyList()
        {
            // Arrange
            var jsonData = string.Empty;

            var parser = new CnbExchangeRateParser();
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // Act
            var exchangeRates = parser.Parse(jsonData, currencies);

            // Assert
            Assert.Empty(exchangeRates);
        }

        [Fact]
        public void Parse_NullCurrency_ReturnsEmptyList()
        {
            // Arrange
            var jsonData = string.Empty;

            var parser = new CnbExchangeRateParser();

            // Act
            var exchangeRates = parser.Parse(jsonData, null);

            // Assert
            Assert.Empty(exchangeRates);
        }
    }
}
