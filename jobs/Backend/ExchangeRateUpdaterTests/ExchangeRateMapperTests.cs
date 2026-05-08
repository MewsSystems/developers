using ExchangeRateUpdater.Infrastructure;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateMapperTests
    {
        private readonly ExchangeRateMapper _parser;

        public ExchangeRateMapperTests()
        {
            _parser = new ExchangeRateMapper();
        }

        [Fact]
        public void Parse_ShouldReturnsExchangeRates_WhenDataIsValid()
        {
            // Arrange
            var data = "header\nCountry|Currency|Amount|Code|Rate\nCzech Republic|Koruna|1|CZK|1.000\nUSA|Dollar|1|USD|23.456";

            // Act
            var result = _parser.Map(data.AsSpan());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Length);
            Assert.Equal("CZK/CZK=1,000", result.Value[0].ToString());
            Assert.Equal("CZK/USD=23,456", result.Value[1].ToString());
        }

        [Fact]
        public void Parse_ShouldReturnsEmpty_WhenDataIsEmpty()
        {
            // Arrange
            var data = "";

            // Act
            var result = _parser.Map(data.AsSpan());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Value);
        }

        [Fact]
        public void Parse_ShouldReturnFailure_WhenDataIsInvalid()
        {
            // Arrange
            var data = "header\nheader\nCountry|Currency|Amount|Code|Rate\nUSA|Dollar|1|USD|23.456";

            // Act
            var result = _parser.Map(data.AsSpan());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Error);
        }
    }
}
