using ExchangeRateUpdater.BusinessLogic.Models;

namespace ExchangeRateUpdaterTests
{
    public class CurrencyTests
    {
        [Fact]
        public void WhenCurrencyIsCreated_With3LettersCode_ShouldBeCreatedWithUpperCase()
        {
            var expectedCode = "eur";

            var currency = new Currency(expectedCode);

            Assert.True(currency.Code == expectedCode.ToUpperInvariant());
        }

        [Fact]
        public void WhenCurrencyIsCreated_WithNo3LettersCode_ShouldThrowError()
        {
            var expectedCode = "eu";

            Assert.Throws<ArgumentException>(() => new Currency(expectedCode));
        }
    }
}