using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Test
{
    [TestFixture]
    public class ExchangeRateProviderTest
    {
        [Test]
        public void Can_Get_Values()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("EUR"), new Currency("AUD") };
            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("EUR"), new decimal(0.94)),
                new ExchangeRate(new Currency("USD"), new Currency("AUD"), new decimal(0.9))
            };
            var mock = new Mock<IBankApi>();
            mock.Setup(x => x.GetValues(currencies)).Returns(exchangeRates);
            var bankApi = new ExchangeRateProvider(mock.Object);

            // Act
            var result = bankApi.GetExchangeRates(currencies);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void Can_Get_NullReferenceException()
        {
            // Arrange
            var mock = new Mock<IBankApi>();
            var bankApi = new ExchangeRateProvider(mock.Object);

            // Act
            Assert.Throws<NullReferenceException>(() => bankApi.GetExchangeRates(null));
        }

        [Test]
        public void Can_Get_ArgumentNullException()
        {
            // Arrange
            var currencies = new List<Currency>();
            var mock = new Mock<IBankApi>();
            var bankApi = new ExchangeRateProvider(mock.Object);

            // Act
            Assert.Throws<ArgumentNullException>(() => bankApi.GetExchangeRates(currencies));
        }
    }
}
