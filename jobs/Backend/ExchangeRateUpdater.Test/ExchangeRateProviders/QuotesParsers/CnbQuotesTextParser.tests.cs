using ExchangeRateUpdater.CoreClasses;
using ExchangeRateUpdater.ExchangeRateProviders.Interfaces;
using ExchangeRateUpdater.ExchangeRateProviders.QuotesParsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Test.ExchangeRateProviders.QuotesParsers
{
    [TestFixture]
    public class CnbQuotesTextParser_tests
    {
        IQuotesParser textParser;
        Currency targetCurrency;

        [SetUp]
        public void Init()
        {
            textParser = new CnbQuotesTextParser();
            targetCurrency = new Currency("CZK");
        }

        [Test]
        public void ParseQuotes_Exceptions_tests()
        {
            string quotes = "01 Oct 2021 #190\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|15.834";

            Assert.Throws<ArgumentNullException>(() => textParser.ParseQuotes(null, quotes));
            Assert.Throws<ArgumentNullException>(() => textParser.ParseQuotes(targetCurrency, null));
            Assert.Throws<ArgumentException>(() => textParser.ParseQuotes(targetCurrency, " "));
        }

        [Test]
        public void ParseQuotesCheckForStructure_Exceptions()
        {
            string quotesWrongDate = "aa bbb cccc #190\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|15.834";
            string quotesWrongHeaders = "01 Oct 2021 #190\nCountry|Currency|Rate|Code|Amount\nAustralia|dollar|1|AUD|15.834";
            string quotesCorrect = "01 Oct 2021 #190\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|15.834";

            Assert.Throws<Exception>(() => textParser.ParseQuotes(targetCurrency, quotesWrongDate));
            Assert.Throws<Exception>(() => textParser.ParseQuotes(targetCurrency, quotesWrongHeaders));
            Assert.DoesNotThrow(() => textParser.ParseQuotes(targetCurrency, quotesCorrect));
        }

        [Test]
        public void ParseQuotes_Success()
        {
            var sourceCurrencyExpectedText = "AUD";
            var sourceAmountExpected = 1;
            var rateExpected = 15.834m;
            var targetAmountExpected = 1;

            string quotesCorrect = $"01 Oct 2021 #190\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|{sourceAmountExpected}|{sourceCurrencyExpectedText}|{rateExpected}";
            var sourceCurrencyExpected = new Currency(sourceCurrencyExpectedText);

            var rate = textParser.ParseQuotes(targetCurrency, quotesCorrect);

            Assert.AreEqual(1, rate.Count);
            Assert.IsTrue(rate.ContainsKey(sourceCurrencyExpected));

            Assert.AreEqual(sourceCurrencyExpected, rate[sourceCurrencyExpected].SourceCurrency);
            Assert.AreEqual(sourceAmountExpected, rate[sourceCurrencyExpected].SourceAmount);
            Assert.AreEqual(targetCurrency, rate[sourceCurrencyExpected].TargetCurrency);
            Assert.AreEqual(targetAmountExpected, rate[sourceCurrencyExpected].TargetAmount);
            Assert.AreEqual(rateExpected, rate[sourceCurrencyExpected].Value);
        }
    }
}
