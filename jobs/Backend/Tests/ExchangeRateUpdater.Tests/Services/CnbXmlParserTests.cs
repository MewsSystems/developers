using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests.Services
{
    public class CnbXmlParserTests
    {
        private const string ValidXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                                            <kurzy>
                                              <tabulka>
                                                <radek kod=""USD"" mnozstvi=""100"" kurz=""24,50"" />
                                                <radek kod=""EUR"" mnozstvi=""1"" kurz=""26,10"" />
                                                <radek kod=""GBP"" mnozstvi=""1"" kurz=""30,00"" />
                                              </tabulka>
                                            </kurzy>";

        [Fact]
        public void Parse_ValidXmlWithRequestedCurrencies_ReturnsExpectedRates()
        {
            var requestedCurrencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            var loggerMock = new Mock<ILogger<CnbXmlParser>>();
            var parser = new CnbXmlParser(loggerMock.Object);

            var result = parser.Parse(ValidXml, requestedCurrencies);

            // Assert
            Assert.Collection(result,
                r =>
                {
                    Assert.Equal("CZK", r.SourceCurrency.Code);
                    Assert.Equal("USD", r.TargetCurrency.Code);
                    Assert.Equal(0.245m, r.Value);
                },
                r =>
                {
                    Assert.Equal("CZK", r.SourceCurrency.Code);
                    Assert.Equal("EUR", r.TargetCurrency.Code);
                    Assert.Equal(26.10m, r.Value);
                });
        }

        [Fact]
        public void Parse_InvalidXml_ThrowsException()
        {
            // Arrange
            var invalidXml = "<invalid>";
            var requestedCurrencies = new List<Currency> { new Currency("USD") };
            var loggerMock = new Mock<ILogger<CnbXmlParser>>();
            var parser = new CnbXmlParser(loggerMock.Object);

            // Act & Assert
            Assert.ThrowsAny<System.Xml.XmlException>(() =>
                parser.Parse(invalidXml, requestedCurrencies));
        }
    }
}
