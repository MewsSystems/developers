using ExchangeRateUpdater;
using ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer;
using ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer.Abstract;
using ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Strategies.CurrencyLayer
{
    [TestClass]
    public class CurrencyLayerRatesProviderTests
    {
        [TestMethod]
        public void ShouldReturnCorrectExchangeRates()
        {
            var mockFetcher = new Mock<ICurrencyLayerRatesFetcher>();
            mockFetcher
                .Setup(o => o.FetchRatesAsync(CurrencyLayerExchangeRateProviderStrategy.ApiKey, new[] { "CZK", "EUR", "GBP" }))
                .Returns(Task.FromResult(new CurrencyLayerResponse
                {
                    Success = true,
                    Source = "USD",
                    Quotes = new Dictionary<string, decimal>
                    {
                        {"USDCZK", 1},
                        {"USDEUR", 2},
                        {"USDGBP", 3}
                    }
                }));
            var provider = new CurrencyLayerExchangeRateProviderStrategy(mockFetcher.Object);

            var rates = provider
                .GetExchangeRatesAsync(new Currency("USD"), new[]
                {
                    new Currency("CZK"),
                    new Currency("EUR"),
                    new Currency("GBP")
                })
                .GetAwaiter()
                .GetResult()
                .ToList();

            Assert.AreEqual(3, rates.Count);

            Assert.AreEqual("USD", rates[0].SourceCurrency.Code);
            Assert.AreEqual("CZK", rates[0].TargetCurrency.Code);
            Assert.AreEqual(1, rates[0].Value);
        }
    }
}
