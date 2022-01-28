using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdaterTests
{
    public class CzechNationalBankExchangeRateTests
    {
        [Theory]
        [InlineData("Australia|dollar|1|AUD|16.584", "AUD", 16.584)]
        [InlineData("Australia|dollar|100|AUD|16.584", "AUD", 0.16584)]
        public void Parse_ValidData(string validData, string expectedCurrencyCode, decimal expectedValue)
        {
            var exchangeRate = new CzechNationalBankExchangeRate(validData);

            Assert.True(exchangeRate.IsValidExchangeRate);
            Assert.NotNull(exchangeRate.ExchangeRate);
            Assert.Equal(expectedCurrencyCode, exchangeRate.ExchangeRate.SourceCurrency.Code);
            Assert.Equal("CZK", exchangeRate.ExchangeRate.TargetCurrency.Code);
            Assert.True(exchangeRate.ExchangeRate.Value == expectedValue);
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
        public void Parse_InvalidData(string invalidData)
        {
            var exchangeRate = new CzechNationalBankExchangeRate(invalidData);

            Assert.False(exchangeRate.IsValidExchangeRate);
            Assert.Null(exchangeRate.ExchangeRate);
        }
    }
}
