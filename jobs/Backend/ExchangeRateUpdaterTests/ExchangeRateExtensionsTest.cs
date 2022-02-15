using System.Collections.Generic;
using ExchangeRateUpdater.Structures;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class ExchangeRateExtensionsTest
    {
        [Test]
        public void ComputeExchangeRates_WhenCurrencyIsSpecified_AndWhenAmountIsOne_ExchangeRatesShouldBeParsed()
        {
            CreateValidExchangeRateTable("1", "USD", "21.254")
                .CallExchangeRates("USD")
                .ShouldBe("USD", "CZK", 21.254M);
        }

        [Test]
        public void ComputeExchangeRates_WhenCurrencyIsSpecified_AndWhenAmountIs100_ExchangeRatesShouldBeParsed_AndAmountShouldBeDivided()
        {
            CreateValidExchangeRateTable("100", "JPY", "18.781")
                .CallExchangeRates("JPY")
                .ShouldBe("JPY", "CZK", 0.18781M);
        }
        
        [Test]
        public void ComputeExchangeRates_WhenDifferentCurrencyIsSpecified_NoEchangeRateShouldBeParsed()
        {
            CreateValidExchangeRateTable("1", "USD", "20.974233")
                .CallExchangeRates("EUR")
                .ShouldBeEmpty();
        }
        
        [Test]
        public void ComputeExchangeRates_WhenTwoRatesAreSpecified_TwoEchangeRatesShouldBeParsed()
        {
            CreateValidExchangeRateTable(
                    "1", "USD", "20.974233",
                    "100", "JPY", "18.781")
                .CallExchangeRates("USD", "JPY")
                .ShouldBe(
                    "USD", "CZK", 20.974233M,
                    "JPY", "CZK", 0.18781M);
        }
        
        [TestCase("WrongAmount", Constants.CodeHeaderName, Constants.RateHeaderName)]
        [TestCase(Constants.AmountHeaderName, "WrongCode", Constants.RateHeaderName)]
        [TestCase(Constants.AmountHeaderName, Constants.CodeHeaderName, "WrongRate")]
        public void ComputeExchangeRates_WhenWrongHeaderName_ExceptionShouldBeThrown(
            string amountHeaderName, string codeHeaderName, string rateHeaderName)
        {
            Assert.Catch(() =>
            {
                new Table(new List<string>() { amountHeaderName, codeHeaderName, rateHeaderName }.AsReadOnly(),
                    new List<List<string>>
                    {
                        new List<string> { "1", "EUR", "24.254" }
                    })
                    .CallExchangeRates("EUR");
            });
        }
        
        private static Table CreateValidExchangeRateTable(string amount, string code, string rate)
        {
            return new Table(Constants.ExchangeRateColumnNames,
                new List<List<string>>
                {
                    new List<string> { amount, code, rate }
                });
        }
        
        private static Table CreateValidExchangeRateTable(
            string amount1, string code1, string rate1,
            string amount2, string code2, string rate2)
        {
            return new Table(Constants.ExchangeRateColumnNames,
                new List<List<string>>
                {
                    new List<string> { amount1, code1, rate1 },
                    new List<string> { amount2, code2, rate2 }
                });
        }
    }
}