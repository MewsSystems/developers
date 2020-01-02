using Unity;
using ExchangeRateProvider.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExchangeRateProvider.Tests.Infrastructure;
using System;
using Moq;

namespace ExchangeRateProvider.Tests
{
    [TestClass]
    public class RateProviderIntegrationTests
    {
        [TestMethod]
        public void GetExchangeRates_NoURL_ThrowsException()
        {
            Mock<IRateProviderSettings> settingsMock = new Mock<IRateProviderSettings>();
            settingsMock.Setup(mock => mock.CZKExchangeRateProviderUrl).Returns(string.Empty);

            IRateProvider rateProvider = ContainerConfiguration.Container.Resolve<IRateProvider>(
                new Unity.Resolution.ParameterOverride("rateProviderSettings", settingsMock.Object)
            );

            Assert.ThrowsException<Exception>(() =>
            {
                rateProvider.GetExchangeRates();
            });
        }

        [TestMethod]
        public void GetExchangeRates_InvalidURL_ThrowsException()
        {
            Mock<IRateProviderSettings> settingsMock = new Mock<IRateProviderSettings>();
            settingsMock.Setup(mock => mock.CZKExchangeRateProviderUrl).Returns("Invalid Url");

            IRateProvider rateProvider = ContainerConfiguration.Container.Resolve<IRateProvider>(
                new Unity.Resolution.ParameterOverride("rateProviderSettings", settingsMock.Object)
            );

            Assert.ThrowsException<ArgumentException>(() =>
            {
                rateProvider.GetExchangeRates();
            });
        }

        [TestMethod]
        public void GetExchangeRates_CNBRateUrl_ReturnRates()
        {
            Mock<IRateProviderSettings> settingsMock = new Mock<IRateProviderSettings>();
            settingsMock.Setup(mock => mock.CZKExchangeRateProviderUrl).Returns(
                "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"
            );

            IRateProvider rateProvider = ContainerConfiguration.Container.Resolve<IRateProvider>(
                new Unity.Resolution.ParameterOverride("rateProviderSettings", settingsMock.Object)
            );

            var rates = rateProvider.GetExchangeRates(new DateTime(2020, 1, 1));

            if (rates.ContainsKey("USD"))
            {
                Assert.AreEqual((decimal)22.621, rates["USD"]);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetExchangeRates_CNBRateUrl_ReturnRateWithAmount1()
        {
            Mock<IRateProviderSettings> settingsMock = new Mock<IRateProviderSettings>();
            settingsMock.Setup(mock => mock.CZKExchangeRateProviderUrl).Returns(
                "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"
            );

            IRateProvider rateProvider = ContainerConfiguration.Container.Resolve<IRateProvider>(
                new Unity.Resolution.ParameterOverride("rateProviderSettings", settingsMock.Object)
            );

            var rates = rateProvider.GetExchangeRates(new DateTime(2020, 1, 1));

            if (rates.ContainsKey("PHP"))
            {
                Assert.AreEqual((decimal)(44.661 / 100), rates["PHP"]);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
