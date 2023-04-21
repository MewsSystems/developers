using ExchangeRateUpdater.BusinessLogic.Models;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateTests
    {

        [Theory]
        [InlineData("eur", null)]
        [InlineData(null, "eur")]
        public void WhenExchangeRateIsCreated_With3LettersCode_ShouldBeCreatedWithUpperCase(string currency1Code, string currency2Code)
        {
            var currency1 = string.IsNullOrEmpty(currency1Code) ? null : new Currency(currency1Code);
            var currency2 = string.IsNullOrEmpty(currency2Code) ? null : new Currency(currency2Code);

            Assert.Throws<ArgumentNullException>(() => new ExchangeRate(currency1, currency2, 1));
        }
    }
}