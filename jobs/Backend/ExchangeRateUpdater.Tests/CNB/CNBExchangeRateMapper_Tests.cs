using ExchangeRateUpdater.Exchange_Providers.Provider.CNB;

namespace ExchangeRateUpdater.Tests.CNB
{
    public class CNBExchangeRateMapper_Tests
    {
        /// <summary>
        /// Tests whether the Map method of ExchangeRateMapper_CNB correctly maps CNB_Exchange_Rate to an ExchangeRate object.
        /// </summary>
        [Fact]
        public void Map_CorrectlyMapsExchangeRate()
        {
            // Arrange
            var mapper = new ExchangeRateMapper_CNB();
            var inputExchangeRate = new CNB_Exchange_Rate
            {
                CurrencyCode = "USD",
                Amount = 1,
                Rate = 25.0
            };

            // Act
            var result = mapper.Map(inputExchangeRate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("USD", result.SourceCurrency.Code);
            Assert.Equal("CZK", result.TargetCurrency.Code);
            Assert.Equal(25.0m, result.Value);
        }

        /// <summary>
        /// Tests whether the Map method of ExchangeRateMapper_CNB throws an ArgumentNullException when null input is provided.
        /// </summary>
        [Fact]
        public void Map_NullInput_ShouldReturnException()
        {
            // Arrange
            var mapper = new ExchangeRateMapper_CNB();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => mapper.Map(null));
        }
    }
}
