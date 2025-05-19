using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ExchangeRateUpdater.UnitTests
{
    [TestClass]
    public class TextParserTests
    {
        [TestMethod]
        public void ParseData_WithCorrectInput_ReturnsListOfExchangeRates()
        {
            // Arrange
            IParser parser = new TextParser();
            string input = "15 May 2025 #92\r\nCountry|Currency|Amount|Code|Rate\r\nAustralia|dollar|1|AUD|14.281\r\nBrazil|real|1|BRL|3.960";

            // Act
            IEnumerable<ExchangeRate> exchangeRates = parser.ParseData(input);

            // Assert
            using (new AssertionScope())
            {
                exchangeRates.Should().NotBeEmpty();
                exchangeRates.Should().HaveCount(2);
                exchangeRates.Select(er => er.SourceCurrency.Code).Should().Contain("AUD");
                exchangeRates.Select(er => er.SourceCurrency.Code).Should().Contain("BRL");
            }
        }

        [TestMethod]
        public void ParseData_WithMalformedInput_ThrowsException()
        {
            // Arrange
            IParser parser = new TextParser();
            string input = "15 May 2025 #92\r\nCountry;Currency;Amount;Code;Rate\r\nAustralia;dollar;1;AUD;14.281\r\nBrazil;real;1;BRL;3.960";

            // Act
            Action act = () => parser.ParseData(input);

            // Assert
            act.Should().Throw<Exception>();
        }

        [TestMethod]
        public void ParseData_WithEmptyInput_ThrowsException()
        {
            // Arrange
            IParser parser = new TextParser();

            // Act
            Action act = () => parser.ParseData("");

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void ParseData_WithNullInput_ThrowsException()
        {
            // Arrange
            IParser parser = new TextParser();

            // Act
            Action act = () => parser.ParseData(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
