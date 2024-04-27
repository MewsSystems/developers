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
            string sourceCurrencyCode = "CZK";
            string data = "25.04.2024 #81\n" +
                          "Header1|Header2|Header3|Header4|Header5\n" +
                          "CountryName1|dollar|100|USD|1.25\n" +
                          "CountryName2|euro|50|EUR|0.75";

            // Act
            var exchangeRates = _parser.Parse(sourceCurrencyCode, data);

            // Assert
            Assert.NotNull(exchangeRates);
            Assert.Equal(2, exchangeRates.Count);

            Assert.Equal("CountryName1", exchangeRates[0].CountryName);
            Assert.Equal("dollar", exchangeRates[0].TargetCurrencyName);
            Assert.Equal("CZK", exchangeRates[0].SourceCurrencyCode);
            Assert.Equal(100, exchangeRates[0].Amount);
            Assert.Equal(1.25m, exchangeRates[0].Value);


            Assert.Equal("CountryName2", exchangeRates[1].CountryName);
            Assert.Equal("euro", exchangeRates[1].TargetCurrencyName);
            Assert.Equal("CZK", exchangeRates[1].SourceCurrencyCode);
            Assert.Equal(50, exchangeRates[1].Amount);
            Assert.Equal(0.75m, exchangeRates[1].Value);
        }

        [Fact]
        public void Parse_SkipsExchangeRate_WhenDataIsMissing()
        {
            // Arrange
            string sourceCurrencyCode = "CZK";
            string data = "25.04.2024 #81\n" +
                          "Header1|Header2|Header3|Header4|Header5\n" +
                          "CountryName1|dollar|100|USD|1.25\n" +
                          "CountryName2|euro|100|EUR"; // MissingData

            // Act
            var exchangeRates = _parser.Parse(sourceCurrencyCode, data);

            // Assert
            Assert.NotNull(exchangeRates);
            Assert.Single(exchangeRates);

            Assert.Equal("CountryName1", exchangeRates[0].CountryName);
            Assert.Equal("dollar", exchangeRates[0].TargetCurrencyName);
            Assert.Equal("USD", exchangeRates[0].TargetCurrencyCode);
            Assert.Equal("CZK", exchangeRates[0].SourceCurrencyCode);
        }
    }
}