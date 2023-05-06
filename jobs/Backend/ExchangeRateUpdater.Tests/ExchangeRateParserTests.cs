using NUnit.Framework;
using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateParserTests
    {
        [Test]
        public void Parse_ReturnsExpectedExchangeRates()
        {
            // Arrange
            var content = @"05 May 2023 #87
                            Country|Currency|Amount|Code|Rate
                            Australia|dollar|1|AUD|14.283
                            Brazil|real|1|BRL|4.252
                            Bulgaria|lev|1|BGN|11.964
                            Canada|dollar|1|CAD|15.748
                            China|renminbi|1|CNY|3.075
                            Denmark|krone|1|DKK|3.141
                            EMU|euro|1|EUR|23.400
                            Hongkong|dollar|1|HKD|2.708
                            Hungary|forint|100|HUF|6.283";

            var expectedExchangeRates = new List<ExchangeRate>()
            {
                new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 14.283m / 1m),
                new ExchangeRate(new Currency("BRL"), new Currency("CZK"), 4.252m / 1m),
                new ExchangeRate(new Currency("BGN"), new Currency("CZK"), 11.964m / 1m),
                new ExchangeRate(new Currency("CAD"), new Currency("CZK"), 15.748m / 1m),
                new ExchangeRate(new Currency("CNY"), new Currency("CZK"), 3.075m / 1m),
                new ExchangeRate(new Currency("DKK"), new Currency("CZK"), 3.141m / 1m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 23.400m / 1m),
                new ExchangeRate(new Currency("HKD"), new Currency("CZK"), 2.708m / 1m),
                new ExchangeRate(new Currency("HUF"), new Currency("CZK"), 6.283m / 100m)
            };

            // Act
            var result = ExchangeRateParser.Parse(content, Currency.CZK);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedExchangeRates).Using(new ExchangeRateComparer()));
        }


        [Test]
        public void Parse_ReturnsEmptyList_WhenContentIsEmpty()
        {
            // Arrange
            string content = string.Empty;
            var targetCurrency = new Currency("CZK");

            // Act
            var result = ExchangeRateParser.Parse(content, targetCurrency);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Parse_ReturnsEmptyList_WhenContentHasOnlyComments()
        {
            // Arrange
            var content = "# This is a comment\n# This is another comment";
            var targetCurrency = new Currency("CZK");

            // Act
            var result = ExchangeRateParser.Parse(content, targetCurrency);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Parse_ReturnsEmptyList_WhenContentHasOnlyHeader()
        {
            // Arrange
            var content = "Country|Currency|Amount|Code|Rate";
            var targetCurrency = new Currency("CZK");

            // Act
            var result = ExchangeRateParser.Parse(content, targetCurrency);

            // Assert
            Assert.That(result, Is.Empty);
        }



    }
}
