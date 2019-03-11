using NUnit.Framework;
using System;
using System.Linq;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class CnbExchangeRateResponseParserTests
    {
        private readonly CnbExchangeRateResponseParser _cnbExchangeRateResponseParse = new CnbExchangeRateResponseParser();

        [Test]
        public void ParseResponseTest2ElementsEnumerable()
        {
            var response = "07.Mar 2019 #47\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|15.989\nBrazil|real|1|BRL|5.922";
            var targetCurrency = new Currency("CZK");
            var expectedResult = new[]
            {
                new ExchangeRate(new Currency("AUD"), targetCurrency, 15.989m),
                new ExchangeRate(new Currency("BRL"), targetCurrency, 5.922m),
            };

            var actualResult = _cnbExchangeRateResponseParse.ParseResponse(response, targetCurrency);

            CollectionAssert.AreEqual(expectedResult, actualResult.ToArray(), new ExchangeRateComparer());
        }

        [Test]
        public void ParseResponseTestArgumentNullException()
        {
            var targetCurrency = new Currency("CZK");

            Assert.Throws<ArgumentNullException>(() => _cnbExchangeRateResponseParse.ParseResponse(null, targetCurrency));
        }

        [Test]
        public void ParseResponseTestFormatException()
        {
            var targetCurrency = new Currency("CZK");
            var response = "07.Mar 2019 #47\nCountry|Currency|Amount|Code|Rate\nsomeincorrectinput";

            Assert.Throws<FormatException>(() => _cnbExchangeRateResponseParse.ParseResponse(response, targetCurrency).ToArray());
        }
    }
}
