using ExchangeRateUpdater;
using ExchangeRateUpdater.ExchangeRateStrategies.Fixer;
using ExchangeRateUpdater.ExchangeRateStrategies.Fixer.Abstract;
using ExchangeRateUpdater.ExchangeRateStrategies.Fixer.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Strategies.Fixer
{
    [TestClass]
    public class FixerExchangeRatesProviderTests
    {
        [TestMethod]
        public void ShouldReturnCorrectExchangeRates()
        {
            var mockFetcher = new Mock<IFixerRatesFetcher>();
            mockFetcher
                .Setup(o => o.FetchRatesAsync(FixerExchangeRateProviderStrategy.ApiKey, new[] { "CZK", "USD", "GBP" }))
                .Returns(Task.FromResult(new FixerResponse
                {
                    Success = true,
                    Date = DateTime.Now,
                    Base = "EUR",
                    Rates = new Dictionary<string, decimal>
                    {
                        {"CZK", 1},
                        {"USD", 2},
                        {"GBP", 3}
                    }
                }));
            var provider = new FixerExchangeRateProviderStrategy(mockFetcher.Object);

            var rates = provider
                .GetExchangeRatesAsync(new Currency("EUR"), new[]
                {
                    new Currency("CZK"),
                    new Currency("USD"),
                    new Currency("GBP")
                })
                .GetAwaiter()
                .GetResult()
                .ToList();

            Assert.AreEqual(3, rates.Count);

            Assert.AreEqual("EUR", rates[0].SourceCurrency.Code);
            Assert.AreEqual("CZK", rates[0].TargetCurrency.Code);
            Assert.AreEqual(1, rates[0].Value);
        }
    }
}
