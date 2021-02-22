using ExchangeRateUpdater;
using ExchangeRateUpdater.CNB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterTests
{
    [TestClass]
    public class ExchangeRateProviderTests
    {
        [TestMethod]
        public void ShouldProvideRatesWhenRatesAreAvailable()
        {
            // Arrange

            var client = new Mock<ICnbClient>();
            client.Setup(m => m.GetExchangeRates())
                .Returns(new ExchangeRatesCollectionDto
                {
                    Table = new ExchangeRateDto[2]
                    {
                        new ExchangeRateDto
                        {
                            Code = "AUD",
                            Quantity = 1,
                            Rate = 16.733M
                        },
                        new ExchangeRateDto
                        {
                            Code = "BRL",
                            Quantity = 1,
                            Rate = 3.937M
                        }
                    }
                });

            var provider = new ExchangeRateProvider(client.Object);

            // Act

            var result = provider.GetExchangeRates(new Currency[] { new Currency("AUD"), new Currency("BRL"), new Currency("CZK") });

            //  Assert

            Assert.IsNotNull(result);

            var rates = result.ToArray();
            Assert.AreEqual(2, rates.Length);

            Assert.AreEqual("CZK", rates[0].SourceCurrency.Code);
            Assert.AreEqual("AUD", rates[0].TargetCurrency.Code);
            Assert.AreEqual(16.733M, rates[0].Value);

            Assert.AreEqual("CZK", rates[1].SourceCurrency.Code);
            Assert.AreEqual("BRL", rates[1].TargetCurrency.Code);
            Assert.AreEqual(3.937M, rates[1].Value);
        }

        [TestMethod]
        public void ShouldProvideNoRatesWhenNoRatesAreAvailable()
        {
            // Arrange

            var client = new Mock<ICnbClient>();
            client.Setup(m => m.GetExchangeRates())
                .Returns(new ExchangeRatesCollectionDto
                {
                    Table = new ExchangeRateDto[0]
                });

            var provider = new ExchangeRateProvider(client.Object);

            // Act

            var result = provider.GetExchangeRates(new Currency[] { new Currency("AUD"), new Currency("BRL") });

            //  Assert

            Assert.IsNotNull(result);
            var rates = result.ToArray();
            Assert.AreEqual(0, rates.Length);
        }

        [TestMethod]
        public void ShouldFilterRates()
        {
            // Arrange

            var client = new Mock<ICnbClient>();
            client.Setup(m => m.GetExchangeRates())
                .Returns(new ExchangeRatesCollectionDto
                {
                    Table = new ExchangeRateDto[2]
                    {
                        new ExchangeRateDto
                        {
                            Code = "AUD",
                            Quantity = 1,
                            Rate = 16.733M
                        },
                        new ExchangeRateDto
                        {
                            Code = "BRL",
                            Quantity = 1,
                            Rate = 3.937M
                        }
                    }
                });

            var provider = new ExchangeRateProvider(client.Object);

            // Act

            var result = provider.GetExchangeRates(new Currency[] { new Currency("AUD") });

            //  Assert

            Assert.IsNotNull(result);

            var rates = result.ToArray();
            Assert.AreEqual(1, rates.Length);

            Assert.AreEqual("CZK", rates[0].SourceCurrency.Code);
            Assert.AreEqual("AUD", rates[0].TargetCurrency.Code);
            Assert.AreEqual(16.733M, rates[0].Value);
        }

        [TestMethod]
        public void ShouldCalculateRateWhenQuantityIsNot1()
        {
            // Arrange

            var client = new Mock<ICnbClient>();
            client.Setup(m => m.GetExchangeRates())
                .Returns(new ExchangeRatesCollectionDto
                {
                    Table = new ExchangeRateDto[]
                    {
                        new ExchangeRateDto
                        {
                            Code = "IDR",
                            Quantity = 1000,
                            Rate = 1.514M
                        }
                    }
                });

            var provider = new ExchangeRateProvider(client.Object);

            // Act

            var result = provider.GetExchangeRates(new Currency[] { new Currency("IDR") });

            //  Assert

            Assert.IsNotNull(result);

            var rates = result.ToArray();
            Assert.AreEqual(1, rates.Length);

            Assert.AreEqual("CZK", rates[0].SourceCurrency.Code);
            Assert.AreEqual("IDR", rates[0].TargetCurrency.Code);
            Assert.AreEqual(0.001514M, rates[0].Value);
        }
    }
}
