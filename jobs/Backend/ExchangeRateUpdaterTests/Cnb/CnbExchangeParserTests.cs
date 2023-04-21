using ExchangeRateUpdater.BusinessLogic.Cnb.Parsers;
using ExchangeRateUpdater.BusinessLogic.Models;

namespace ExchangeRateUpdaterTests.Cnb
{
    public class CnbExchangeParserTests
    {
        [Theory]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        public void WhenParseCnbFxExchangeRate_WithInvalidNullParam_ShouldThrowAnException(string separatorKey, string resultFxExchange)
        {
            Assert.Throws<ArgumentNullException>(() => CnbExchangeParser.ParseCnbFxExchangeRate(separatorKey, resultFxExchange, 1, 1));
        }

        [Theory]
        [InlineData(",", "test")]
        [InlineData("t", "test")]
        public void WhenParseCnbFxExchangeRate_WithResultOrSeparatorWrong_ShouldReturnNull(string separatorKey, string resultFxExchange)
        {
           var value = CnbExchangeParser.ParseCnbFxExchangeRate(separatorKey, resultFxExchange, 1, 1);

            Assert.Null(value);
        }

        [Theory]
        [InlineData(null, "eur", "test")]
        [InlineData("test", null, "test")]
        [InlineData("test", "eur", null)]
        public void WhenParseCnbExchangeRate_WithInvalidNullParam_ShouldThrowAnException(string separatorKey, string currencyCode, string resultFxExchange)
        {
            var currency  = string.IsNullOrEmpty(currencyCode) ? null : new Currency(currencyCode);
            Assert.Throws<ArgumentNullException>(() => CnbExchangeParser.ParseCnbExchangeRate(separatorKey, currency, resultFxExchange, 1, 1, 1));
        }

        [Theory]
        [InlineData("test", "eur", "test")]
        [InlineData(",", "eur", "test")]
        [InlineData("t", "eur", "test")]
        public void WhenParseCnbExchangeRate_WithResultOrSeparatorWrong_ShouldReturnNull(string separatorKey, string currencyCode, string resultFxExchange)
        {
            var currency = string.IsNullOrEmpty(currencyCode) ? null : new Currency(currencyCode);
            CnbExchangeParser.ParseCnbExchangeRate(separatorKey, currency, resultFxExchange, 1, 1, 1);
        }
    }
}