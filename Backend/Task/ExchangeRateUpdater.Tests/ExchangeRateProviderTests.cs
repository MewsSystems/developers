using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class ExchangeRateProviderTests
    {
        [TestMethod]
        public void ExchangeRateProvider_StandardTest()
        {
            var czk = new Currency("CZK");
            var rates = new[]
            {
                new ExchangeRate(czk, new Currency("AUD"), 16.164M),
                new ExchangeRate(czk, new Currency("BRL"), 6.112M),
                new ExchangeRate(czk, new Currency("BGN"), 13.209M),
                new ExchangeRate(czk, new Currency("CNY"), 3.365M), 
                new ExchangeRate(czk, new Currency("DKK"), 3.461M),
                new ExchangeRate(czk, new Currency("EUR"), 25.835M),
                new ExchangeRate(czk, new Currency("PHP"), 0.43796M),
                new ExchangeRate(czk, new Currency("HKD"), 2.911M), 
                new ExchangeRate(czk, new Currency("HRK"), 3.489M),
                new ExchangeRate(czk, new Currency("INR"), 0.32112M),
                new ExchangeRate(czk, new Currency("IDR"), 0.001627M),
                new ExchangeRate(czk, new Currency("ISK"), 0.18913M),
                new ExchangeRate(czk, new Currency("ILS"), 6.271M),
                new ExchangeRate(czk, new Currency("JPY"), 0.20729M),
                new ExchangeRate(czk, new Currency("ZAR"), 1.663M), 
                new ExchangeRate(czk, new Currency("CAD"), 17.216M),
                new ExchangeRate(czk, new Currency("KRW"), 0.0203M),
                new ExchangeRate(czk, new Currency("HUF"), 0.08082M),
                new ExchangeRate(czk, new Currency("MYR"), 5.609M), 
                new ExchangeRate(czk, new Currency("MXN"), 1.197M),
                new ExchangeRate(czk, new Currency("XDR"), 31.772M), 
                new ExchangeRate(czk, new Currency("NOK"), 2.631M),
                new ExchangeRate(czk, new Currency("NZD"), 15.410M), 
                new ExchangeRate(czk, new Currency("PLN"), 5.986M),
                new ExchangeRate(czk, new Currency("RON"), 5.449M), 
                new ExchangeRate(czk, new Currency("RUB"), 0.3482M),
                new ExchangeRate(czk, new Currency("SGD"), 16.815M), 
                new ExchangeRate(czk, new Currency("SEK"), 2.464M),
                new ExchangeRate(czk, new Currency("CHF"), 22.759M),
                new ExchangeRate(czk, new Currency("THB"), 0.72707M),
                new ExchangeRate(czk, new Currency("TRY"), 4.335M), 
                new ExchangeRate(czk, new Currency("USD"), 22.845M),
                new ExchangeRate(czk, new Currency("GBP"), 29.487M)
            };

            var source = Mock.Create<IRateFeedSource>();
            source.Arrange(x => x.LoadRatesFeedAsync()).Returns(Task.FromResult("not empty feed"));

            var parser = Mock.Create<IRateFeedParser>();
            parser.Arrange(x => x.Parse(Arg.AnyString)).Returns(rates);



            var unit = new ExchangeRateProvider(source, parser);
            
            var input = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"), // not there
                new Currency("JPY"), 
                new Currency("KES"), // not there
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")  // not there
            };

            var result = unit.GetExchangeRates(input).ToArray();

            Assert.AreEqual(6, result.Length);

            Assert.IsTrue(result.Any(x => x.TargetCurrency.Code == "USD"));
            Assert.IsTrue(result.Any(x => x.TargetCurrency.Code == "EUR"));
            Assert.IsFalse(result.Any(x => x.TargetCurrency.Code == "CZK"));
            Assert.IsTrue(result.Any(x => x.TargetCurrency.Code == "JPY"));
            Assert.IsFalse(result.Any(x => x.TargetCurrency.Code == "KES"));
            Assert.IsTrue(result.Any(x => x.TargetCurrency.Code == "RUB"));
            Assert.IsTrue(result.Any(x => x.TargetCurrency.Code == "THB"));
            Assert.IsTrue(result.Any(x => x.TargetCurrency.Code == "TRY"));
            Assert.IsFalse(result.Any(x => x.TargetCurrency.Code == "XYZ"));
        }

        [TestMethod]
        public void ExchangeRateProvider_EmptyFeedTest()
        {

            var source = Mock.Create<IRateFeedSource>();
            source.Arrange(x => x.LoadRatesFeedAsync()).Returns(Task.FromResult(string.Empty));

            var parser = Mock.Create<IRateFeedParser>();



            var unit = new ExchangeRateProvider(source, parser);
            
            var input = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"), // not there
                new Currency("JPY"), 
                new Currency("KES"), // not there
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")  // not there
            };

            var result = unit.GetExchangeRates(input).ToArray();

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void ExchangeRateProvider_EmptyInputTest()
        {
            var source = Mock.Create<IRateFeedSource>();
            var parser = Mock.Create<IRateFeedParser>();
            
            var unit = new ExchangeRateProvider(source, parser);

            var input = new Currency[0];

            var result = unit.GetExchangeRates(input).ToArray();

            Assert.AreEqual(0, result.Length);
        }

        

        [TestMethod]
        public void ExchangeRateProvider_NullInputTest()
        {
            var source = Mock.Create<IRateFeedSource>();
            var parser = Mock.Create<IRateFeedParser>();
            
            var unit = new ExchangeRateProvider(source, parser);

            Assert.ThrowsException<ArgumentNullException>(() => unit.GetExchangeRates(null));
        }
    }
}
