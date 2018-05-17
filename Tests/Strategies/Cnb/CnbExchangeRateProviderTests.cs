using ExchangeRateUpdater;
using ExchangeRateUpdater.ExchangeRateStrategies.Cnb;
using ExchangeRateUpdater.ExchangeRateStrategies.Cnb.Abstract;
using ExchangeRateUpdater.ExchangeRateStrategies.Cnb.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MSTestExtensions;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Strategies.Cnb
{
    [TestClass]
    public class CnbExchangeRateProviderTests
    {
        private CnbExchangeRateProviderStrategy _cnbExchangeRateProvider;

        [TestInitialize]
        public void Setup()
        {
            var mockFetcher = new Mock<ICnbRatesFetcher>();
            mockFetcher
                .Setup(o => o.FetchRatesAsync(CnbExchangeRateProviderStrategy.CnbRatesUrl))
                .Returns(Task.FromResult("fake contents"));

            var mockParser = new Mock<ICnbRatesParser>();
            mockParser
                .Setup(o => o.Parse("fake contents"))
                .Returns(new[]
                {
                    new CnbRate("AUD", 1, 16.229m),
                    new CnbRate("JPY", 100, 19.677m)
                });

            _cnbExchangeRateProvider = new CnbExchangeRateProviderStrategy(mockFetcher.Object, mockParser.Object);
        }


        [TestMethod]
        public void ShouldFailWithNotCzkBaseCurrency()
        {
            ThrowsAsyncAssert.ThrowsAsync(_cnbExchangeRateProvider
                .GetExchangeRatesAsync(new Currency("USD"), new[]
                {
                    new Currency("EUR")
                }), "CNB exchange rate provider requires CZK as target currency");
        }

        [TestMethod]
        public void ShouldReturnCorrectExchangeRates()
        {
            var rates = _cnbExchangeRateProvider
                .GetExchangeRatesAsync(new Currency("CZK"), new[]
                {
                    new Currency("EUR"),
                    new Currency("JPY")
                })
                .GetAwaiter()
                .GetResult()
                .ToList();

            Assert.AreEqual(1, rates.Count);

            Assert.AreEqual("JPY", rates[0].SourceCurrency.Code);
            Assert.AreEqual("CZK", rates[0].TargetCurrency.Code);
            Assert.AreEqual(0.19677m, rates[0].Value);
        }
    }
}
