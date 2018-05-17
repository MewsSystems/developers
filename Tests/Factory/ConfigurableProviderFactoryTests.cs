using ExchangeRateUpdater;
using ExchangeRateUpdater.ExchangeRateStrategies;
using ExchangeRateUpdater.Factory;
using ExchangeRateUpdater.Factory.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Factory
{
    [TestClass]
    public class ConfigurableProviderFactoryTests
    {
        private IExchangeRateProviderSourceCurrencyStrategy _czkSourceCurrencyProvider;
        private IExchangeRateProviderTargetCurrencyStrategy _eurTargetCurrencyProvider;
        private Mock<IProviderFactoryConfig> _mockConfig;

        [TestInitialize]
        public void Setup()
        {
            _czkSourceCurrencyProvider = new Mock<IExchangeRateProviderSourceCurrencyStrategy>().Object;
            _eurTargetCurrencyProvider = new Mock<IExchangeRateProviderTargetCurrencyStrategy>().Object;

            _mockConfig = new Mock<IProviderFactoryConfig>();
            _mockConfig
                .Setup(o => o.SourceCurrencyProviderFactories)
                .Returns(new Dictionary<Currency, Func<IExchangeRateProviderSourceCurrencyStrategy>>
                {
                    {new Currency("CZK"), () => _czkSourceCurrencyProvider}
                });
            _mockConfig
                .Setup(o => o.TargetCurrencyProviderFactories)
                .Returns(new Dictionary<Currency, Func<IExchangeRateProviderTargetCurrencyStrategy>>
                {
                    {new Currency("EUR"), () => _eurTargetCurrencyProvider}
                });
        }

        [TestMethod]
        public void ShouldReturnSourceCurrencyProvider()
        {
            var factory = new ConfigurableProviderFactory(_mockConfig.Object);

            var provider = factory.GetSourceCurrencyProvider(new Currency("CZK"));

            Assert.AreSame(_czkSourceCurrencyProvider, provider);
        }

        [TestMethod]
        public void ShouldReturnTargetCurrencyProvider()
        {
            var factory = new ConfigurableProviderFactory(_mockConfig.Object);

            var provider = factory.GetTargetCurrencyProvider(new Currency("EUR"));

            Assert.AreSame(_eurTargetCurrencyProvider, provider);
        }

        [TestMethod]
        public void ShouldReturnNullCurrencyProvider()
        {
            var factory = new ConfigurableProviderFactory(_mockConfig.Object);

            var sourceProvider = factory.GetSourceCurrencyProvider(new Currency("USD"));
            var targetProvider = factory.GetTargetCurrencyProvider(new Currency("USD"));

            Assert.IsInstanceOfType(sourceProvider, typeof(NullExchangeRateProviderStrategy));
            Assert.IsInstanceOfType(targetProvider, typeof(NullExchangeRateProviderStrategy));
        }

        [TestMethod]
        public void ShouldReturnProviders()
        {
            var factory = new ConfigurableProviderFactory(_mockConfig.Object);

            var providers = factory
                .GetProviders(new[]
                {
                    new Currency("EUR"),
                    new Currency("USD")
                })
                .ToList();

            Assert.AreEqual(1, providers.Count);
            Assert.AreEqual("EUR", providers[0].BaseCurrency.Code);
            Assert.AreSame(_eurTargetCurrencyProvider, providers[0].Provider);
        }
    }
}
