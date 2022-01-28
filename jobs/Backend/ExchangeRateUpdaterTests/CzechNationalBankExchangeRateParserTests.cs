using ExchangeRateUpdater.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdaterTests
{
    public class CzechNationalBankExchangeRateParserTests
    {
        [Theory]
        [InlineData("Australia|dollar|1|AUD|16.584", "AUD", 16.584)]
        [InlineData("Australia|dollar|100|AUD|16.584", "AUD", 0.16584)]
        public void ConvertToExchangeRatesAsync_ValidData_IsReturned(string validData, string expectedCurrencyCode, decimal expectedValue)
        {
            var parser = new CzechNationalBankExchangeRateParser();
            var linesWithExchangeRates = new List<string>() { validData };

            var exchangeRates = parser.ConvertToExchangeRates(linesWithExchangeRates.ToAsyncEnumerable());

            Assert.Single(exchangeRates);
            Assert.Equal(expectedCurrencyCode, exchangeRates.Single().SourceCurrency.Code);
            Assert.Equal("CZK", exchangeRates.Single().TargetCurrency.Code);
            Assert.True(exchangeRates.Single().Value == expectedValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData("25 Jan 2021 #16")]
        [InlineData("Country|Currency|Amount|Code|Rate")]
        [InlineData("Australia|dollar|-1|AUD|16.584")]
        [InlineData("Australia|dollar|0|AUD|16.584")]
        [InlineData("Australia|dollar|error|AUD|16.584")]
        [InlineData("Australia|dollar|1|AD|16.584")]
        [InlineData("Australia|dollar|1|AUDA|16.584")]
        [InlineData("Australia|dollar|1|AUD|-16.584")]
        [InlineData("Australia|dollar|1|AUD|0")]
        [InlineData("Australia|dollar|1|AUD|error")]
        [InlineData("Australia|dollar|1AUD|16.584")]
        [InlineData("Australia|dollar|1|AUD|16.584|")]
        public void ConvertToExchangeRatesAsync_InvalidData_IsIgnored(string invalidData)
        {
            var parser = new CzechNationalBankExchangeRateParser();
            var linesWithExchangeRates = new List<string>() { invalidData };

            var exchangeRates = parser.ConvertToExchangeRates(linesWithExchangeRates.ToAsyncEnumerable());

            Assert.Empty(exchangeRates);
        }

        [Fact]
        public void ConvertToExchangeRatesAsync_NoData_ReturnsEmptyList()
        {
            var parser = new CzechNationalBankExchangeRateParser();
            var linesWithExchangeRates = new List<string>() { };

            var exchangeRates = parser.ConvertToExchangeRates(linesWithExchangeRates.ToAsyncEnumerable());

            Assert.Empty(exchangeRates);
        }
    }
}
