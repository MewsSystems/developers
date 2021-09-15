using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExchangeRateUpdater.Parsing;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class ExchangeRateProviderTests
    {
        [TestMethod]
        public void ExchangeRateProviderTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ExchangeRateProvider(null, Substitute.For<IExchangeRateParser>()));

            Assert.ThrowsException<ArgumentNullException>(() => new ExchangeRateProvider(Substitute.For<ICommunicator>(), null));

            Assert.IsNotNull(new ExchangeRateProvider(Substitute.For<ICommunicator>(), Substitute.For<IExchangeRateParser>()));
        }

        [TestMethod]
        [DynamicData(nameof(GetExchangeRatesTestData))]
        public async Task GetExchangeRatesTest(IEnumerable<Currency> currencies, List<ExchangeRate> expectedResult)
        {
            var communicator = Substitute.For<ICommunicator>();
            var parser = Substitute.For<IExchangeRateParser>();
            var provider = new ExchangeRateProvider(communicator, parser);

            communicator.GetExchangeRateData().Returns(Task.FromResult("data"));

            parser.Parse(Arg.Any<string>()).Returns(new List<ExchangeRate>()
            {
                new ExchangeRate(new Currency("SEK"), new Currency("GBP"), 10, 1),
                new ExchangeRate(new Currency("GBP"), new Currency("CZK"), 10.5M, 1),
                new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 25.6M, 1),
                new ExchangeRate(new Currency("DKK"), new Currency("USD"), 1.3M, 1),
                new ExchangeRate(new Currency("CAD"), new Currency("USD"), 1.5M, 1)
            });
            var actualResult = (await provider.GetExchangeRates(currencies)).ToList();

            Received.InOrder(async () =>
            {
                await communicator.GetExchangeRateData();
            });

            parser.Received(1).Parse(Arg.Is("data"));

            Assert.AreEqual(expectedResult.Count, actualResult.Count());

            if (expectedResult.Any())
            {
                for (int i = 0; i < expectedResult.Count; i++)
                {
                    Assert.IsTrue(expectedResult[i].Equals(actualResult[i]), $"Exchange rates are not equal: {expectedResult[i]} : {actualResult[i]}");
                }
            }
        }

        public static IEnumerable<object[]> GetExchangeRatesTestData
        {
            get
            {
                yield return new object[] { new List<Currency> { }, new List<ExchangeRate>() { } };
                yield return new object[] { new List<Currency> { new Currency("ABC") }, new List<ExchangeRate>() { } };
                yield return new object[] { new List<Currency> { new Currency("aud") }, new List<ExchangeRate>() { new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 25.6M, 1) } };
                yield return new object[] 
                {
                    new List<Currency>
                    {
                        new Currency("cad"),
                        new Currency("xxx"),
                        new Currency("dkk"),
                        new Currency(null)
                    },
                    new List<ExchangeRate>()
                    {
                        new ExchangeRate(new Currency("CAD"), new Currency("USD"), 1.5M, 1),
                        new ExchangeRate(new Currency("DKK"), new Currency("USD"), 1.3M, 1)
                    }
                };
            }
        }
    }
}