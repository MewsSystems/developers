using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Tests.Helpers
{
    public class ExchangeRateHelperTests
    {
        [Test]
        public void ConvertToExchangeRates_NullInput_ReturnsNull()
        {
            // Arrange && Act
            var result = ExchangeRateHelper.ConvertToExchangeRates(null, null);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ConvertToExchangeRates_EmptyInput_ReturnsNull()
        {
            // Arrange && Act
            var rates = new List<ThirdPartyExchangeRate>();
            var result = ExchangeRateHelper.ConvertToExchangeRates(rates, null);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ConvertToExchangeRates_ValidInput_ReturnsList()
        {
            // Arrange && Act
            var rates = GetThirdPartyExchangeRates();
            var result = ExchangeRateHelper.ConvertToExchangeRates(rates, null);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(rates.Count));
            Assert.That(result.All(x => x.SourceCurrency.Code == "CZK"), Is.True);
        }

        #region Test data helpers

        public List<ThirdPartyExchangeRate> GetThirdPartyExchangeRates()
        {
            return new List<ThirdPartyExchangeRate>
            {
                new ThirdPartyExchangeRate {Country = "Australia", Currency = "dollar", Amount = 1, CurrencyCode = "AUD", Rate = 14.529M },
                new ThirdPartyExchangeRate {Country = "EMU", Currency = "euro", Amount = 1, CurrencyCode = "EUR", Rate = 23.635M },
                new ThirdPartyExchangeRate {Country = "United Kingdom", Currency = "pound", Amount = 1, CurrencyCode = "GBP", Rate = 27.203M }
            };
        }

        #endregion
    }
}
