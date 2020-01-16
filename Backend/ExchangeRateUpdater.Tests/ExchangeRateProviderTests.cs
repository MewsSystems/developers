using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class ExchangeRateProviderTests
    {
        [TestMethod]
        public void TwoCorrectValues_Success()
        {
            var testCurrencies = new[]
            {
                TestCurrencies.American,
                TestCurrencies.Espagnol
            };

            var exchangeRates = new ExchangeRateProvider(new DummyLoaderWithValues())
                .GetExchangeRates(testCurrencies)
                .ToList();

            var czkToUsd = exchangeRates.FirstOrDefault(e => e.TargetCurrency.Code == "USD");
            var czkToEsp = exchangeRates.FirstOrDefault(e => e.TargetCurrency.Code == "ESP");

            Assert.AreEqual(2, exchangeRates.Count);

            Assert.IsNotNull(czkToUsd);
            Assert.IsNotNull(czkToEsp);

            Assert.AreEqual(24.5M, czkToUsd.Value);
            Assert.AreEqual(12.1M, czkToEsp.Value);
        }

        [TestMethod]
        public void OneCorrectValue_Success()
        {
            var testCurrencies = new[]
            {
                TestCurrencies.American,
                TestCurrencies.ImaginationLand
            };

            var exchangeRates = new ExchangeRateProvider(new DummyLoaderWithValues())
                .GetExchangeRates(testCurrencies)
                .ToList();

            var czkToUsd = exchangeRates.FirstOrDefault(e => e.TargetCurrency.Code == "USD");

            Assert.AreEqual(1, exchangeRates.Count);

            Assert.IsNotNull(czkToUsd);

            Assert.AreEqual(24.5M, czkToUsd.Value);
        }

        [TestMethod]
        public void OneCorrectValueOneNullValue_Success()
        {
            var testCurrencies = new[]
            {
                TestCurrencies.American,
                null
            };

            var exchangeRates = new ExchangeRateProvider(new DummyLoaderWithValues())
                .GetExchangeRates(testCurrencies)
                .ToList();

            var czkToUsd = exchangeRates.FirstOrDefault(e => e.TargetCurrency.Code == "USD");

            Assert.AreEqual(1, exchangeRates.Count);

            Assert.IsNotNull(czkToUsd);

            Assert.AreEqual(24.5M, czkToUsd.Value);
        }

        [TestMethod]
        public void NoCorrectValues_Success()
        {
            var testCurrencies = new[]
            {
                TestCurrencies.ImaginationLand
            };

            var exchangeRates = new ExchangeRateProvider(new DummyLoaderWithValues())
                .GetExchangeRates(testCurrencies)
                .ToList();

            Assert.AreEqual(0, exchangeRates.Count);
        }

        [TestMethod]
        public void EmptyInput_Success()
        {
            var exchangeRates = new ExchangeRateProvider(new DummyLoaderWithValues())
                .GetExchangeRates(Enumerable.Empty<Currency>())
                .ToList();

            Assert.AreEqual(0, exchangeRates.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullInput_Fail()
        {
            var exchangeRates = new ExchangeRateProvider(new DummyLoaderWithValues())
                .GetExchangeRates(null)
                .ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullArgument_Fail()
        {
            var exchangeRates = new ExchangeRateProvider(null)
                .GetExchangeRates(null)
                .ToList();
        }

        [TestMethod]
        public void LoaderWithNoValues_Success()
        {
            var testCurrencies = new[]
            {
                TestCurrencies.American,
                TestCurrencies.Espagnol
            };

            var exchangeRates = new ExchangeRateProvider(new DummyLoaderWithoutValues())
                .GetExchangeRates(testCurrencies)
                .ToList();

            Assert.AreEqual(0, exchangeRates.Count);
        }

        private class DummyLoaderWithValues : IExchangeRateLoader
        {
            public Task<IReadOnlyCollection<ExchangeRate>> LoadExchangeRates()
            {
                return Task.FromResult((IReadOnlyCollection<ExchangeRate>)
                    new List<ExchangeRate>
                    {
                        new ExchangeRate(Currency.Czech, new Currency("USD"), 24.5M),
                        new ExchangeRate(Currency.Czech, new Currency("ESP"), 12.1M),
                    });
            }
        }

        private class DummyLoaderWithoutValues : IExchangeRateLoader
        {
            public Task<IReadOnlyCollection<ExchangeRate>> LoadExchangeRates()
            {
                return Task.FromResult((IReadOnlyCollection<ExchangeRate>)
                    new List<ExchangeRate>());
            }
        }
    }
}
