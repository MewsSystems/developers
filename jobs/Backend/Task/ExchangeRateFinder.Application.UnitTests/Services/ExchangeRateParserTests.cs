namespace ExchangeRateFinder.Application.UnitTests.Services
{
    using ExchangeRateFinder.Infrastructure.Services;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ExchangeRateParserTests
    {
        private readonly ExchangeRateParser _parser;
        private readonly Mock<ILogger<ExchangeRateParser>> _loggerMock;

        public ExchangeRateParserTests()
        {
            _loggerMock = new Mock<ILogger<ExchangeRateParser>>();
            _parser = new ExchangeRateParser(_loggerMock.Object);
        }

        [Fact]
        public void Parse_ReturnsExchangeRates_WhenDataIsValid()
        {
            // Arrange
            string sourceCurrency = "CZK";
            string data = "25.04.2024 #81\n" +
                          "Header1|Header2|Header3|Header4|Header5\n" +
                          "Country1|dollar|100|USD|1.25\n" +
                          "Country2|euro|50|EUR|0.75";

            // Act
            var exchangeRates = _parser.Parse(sourceCurrency, data);

            // Assert
            Assert.NotNull(exchangeRates);
            Assert.Equal(2, exchangeRates.Count);

            Assert.Equal("Country1", exchangeRates[0].Country);
            Assert.Equal("dollar", exchangeRates[0].TargetCurrency);
            Assert.Equal("CZK", exchangeRates[0].SourceCurrency);
            Assert.Equal(100, exchangeRates[0].Amount);
            Assert.Equal("USD", exchangeRates[0].CurrencyCode);
            Assert.Equal(1.25m, exchangeRates[0].Rate);


            Assert.Equal("Country2", exchangeRates[1].Country);
            Assert.Equal("euro", exchangeRates[1].TargetCurrency);
            Assert.Equal(50, exchangeRates[1].Amount);
            Assert.Equal("EUR", exchangeRates[1].CurrencyCode);
            Assert.Equal(0.75m, exchangeRates[1].Rate);
            Assert.Equal("CZK", exchangeRates[1].SourceCurrency);
        }

        [Fact]
        public void Parse_SkipsExchangeRates_WhenDataIsMissing()
        {
            // Arrange
            string sourceCurrency = "CZK";
            string data = "25.04.2024 #81\n" +
                          "Header1|Header2|Header3|Header4|Header5\n" +
                          "Country1|dollar|100|USD|1.25\n" +
                          "Country2|euro|100|EUR"; // MissingData

            // Act
            var exchangeRates = _parser.Parse(sourceCurrency, data);

            // Assert
            Assert.NotNull(exchangeRates);
            Assert.Single(exchangeRates);

            Assert.Equal("Country1", exchangeRates[0].Country);
            Assert.Equal("dollar", exchangeRates[0].TargetCurrency);
            Assert.Equal("CZK", exchangeRates[0].SourceCurrency);
        }
    }
}