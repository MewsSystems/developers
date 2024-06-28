using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Tests.UnitTests.Domain
{
    public sealed class ExchangeRateCurrencyTests
    {
        //### US 1 - Currency can have any code
        //As a Currency
        //I want to be able to get the currency code directly from object
        //So that I can quickly convert it

        //**UAT1**
        //Given the currency is CZK
        //When the currency is formatted ToString()
        //Then the value is token is "CZK"
        [Theory]
        [InlineData("CZK", "CZK")]
        public void US_1_UAT1(string input, string expected)
        {
            var sut = new Currency(input);

            Assert.Equal(expected, sut.ToString());
        }

        //### US 2 - Exchange rate must show the source currency, target currency and rate value
        //As a exchange Rate
        //I want to be able to format my exchange rate in readable text
        //So that I can understand the exchange rate

        //**UAT1**
        //Given the source currency is CZK
        //And the target currency is USD
        //And the rate value is 22.159
        //When the ExchangeRate is formatted ToString()
        //Then the value is "CZK/USD=22,159"
        [Theory]
        [InlineData("CZK", "USD", 22.159, "CZK/USD=22,159")]
        public void US_2_UAT1(string inputSourceCurrency, string inputTargetCurrency, decimal inputValue, string expected)
        {
            var sourceCurrency = new Currency(inputSourceCurrency);
            var targetCurrency = new Currency(inputTargetCurrency);
            var sut = new ExchangeRate(sourceCurrency, targetCurrency, inputValue);

            Assert.Equal(expected, sut.ToString());
        }
    }
}
