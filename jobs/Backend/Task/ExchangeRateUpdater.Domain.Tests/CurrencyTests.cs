namespace ExchangeRateUpdater.Domain.Tests
{
    public class CurrencyTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("AB")]
        [InlineData("ABCD")]
        [InlineData(null)]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsNotThreeLetters(string? invalidCode)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Currency(invalidCode));
        }

        [Theory]
        [InlineData("usd", "USD")]
        [InlineData("cZk", "CZK")]
        [InlineData("EuR", "EUR")]
        public void Constructor_ShouldConvertCodeToUppercase(string? inputCode, string expectedCode)
        {
            // Act
            var currency = new Currency(inputCode);

            // Assert
            Assert.Equal(expectedCode, currency.Code);
        }
    }
}
