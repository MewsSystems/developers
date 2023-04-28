using ExchangeRateUpdaterAPI.Services.ExchangeRateFormatterService;

namespace ExchangeRateUpdaterAPITests
{
    public class ExchangeRateFormatterTests
    {
        private readonly ExchangeRateFormatter _exchangeRateFormatter = new ExchangeRateFormatter();

        [Test]
        public void FormatExchangeRates_WhenDataIsValid_ReturnsCorrectExchangeRates()
        {
            // Arrange
            string data = @"27 Apr 2023 #82
                            Country|Currency|Amount|Code|Rate
                            USD|dollar|1|USD|1.0000
                            EUR|euro|1|EUR|1.2000
                            JPY|yen|100|JPY|0.9500";

            // Act
            var exchangeRates = _exchangeRateFormatter.FormatExchangeRates(data).ToList();

            // Assert
            Assert.That(exchangeRates.Count, Is.EqualTo(3));
            var usdExchangeRate = exchangeRates.Single(x => x.SourceCurrency.Code == "CZK" && x.TargetCurrency.Code == "USD");
            Assert.That(usdExchangeRate.Value, Is.EqualTo(1.0000m));

            var eurExchangeRate = exchangeRates.Single(x => x.SourceCurrency.Code == "CZK" && x.TargetCurrency.Code == "EUR");
            Assert.That(eurExchangeRate.Value, Is.EqualTo(1.2000m));

            var jpyExchangeRate = exchangeRates.Single(x => x.SourceCurrency.Code == "CZK" && x.TargetCurrency.Code == "JPY");
            Assert.That(jpyExchangeRate.Value, Is.EqualTo(0.95m));
        }

        [Test]
        public void FormatExchangeRates_WhenDataIsMissingColumns_ThrowsFormatException()
        {
            // Arrange
            string data = @"27 Apr 2023 #82
                            Country|Currency|Amount|Code|Rate
                            USD|dollar|1|USD|1.0000
                            EUR|euro|1|EUR
                            JPY|yen|100|JPY|0.9500";

            // Act & Assert
            Assert.Throws<FormatException>(() => _exchangeRateFormatter.FormatExchangeRates(data).ToList());
        }

        [Test]
        public void FormatExchangeRates_WhenDataContainsInvalidDecimal_ThrowsFormatException()
        {
            // Arrange
            string data = @"27 Apr 2023 #82
                            Country|Currency|Amount|Code|RateUSD|dollar|1|USD|1.0000
                            EUR|euro|1|EUR|1.2.2
                            JPY|yen|100|JPY|0.9500";

            // Act & Assert
            Assert.Throws<FormatException>(() => _exchangeRateFormatter.FormatExchangeRates(data).ToList());
        }

        [Test]
        public void FormatExchangeRates_WhenDataIsEmpty_ThrowsFormatException()
        {
            // Arrange
            string data = "";

            // Act & Assert
            Assert.Throws<FormatException>(() => _exchangeRateFormatter.FormatExchangeRates(data).ToList());
        }
    }
}

