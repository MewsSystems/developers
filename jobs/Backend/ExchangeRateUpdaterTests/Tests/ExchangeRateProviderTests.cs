using Castle.Core.Configuration;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Providers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterTests
{
    [TestFixture]
    public class ExchangeRateProviderTests
    {
        private Mock<ILogger<ExchangeRateProvider>> _mockLogger;
        private Mock<IExchangeRateProviderFactory> _mockExchangeRateProviderFactory;
        private ExchangeRateProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<ExchangeRateProvider>>();
            _mockExchangeRateProviderFactory = new Mock<IExchangeRateProviderFactory>();

            _provider = new ExchangeRateProvider(_mockExchangeRateProviderFactory.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetExchangeRatesAsync_WhenApiReturnsData_ShouldNotCallTextExchangeRateProvider()
        {
            
            var currencies = new List<Currency> { new Currency("USD") };
            var expected = new List<ExchangeRate> { new ExchangeRate(new Currency("CZK"), new Currency("USD"), 0.04m) };

            _mockExchangeRateProviderFactory.Setup(x => x.Create(ProviderType.api).GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime?>()))
                                        .ReturnsAsync(expected);

          
            var result = await _provider.GetExchangeRatesAsync(currencies, DateTime.Now);


            _mockExchangeRateProviderFactory.Verify(x => x.Create(ProviderType.text).GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime>()), Times.Never);
            result.ShouldBe(expected);
        }

        [Test]
        public async Task GetExchangeRatesAsync_WhenApiReturnsNoData_ShouldCallTextExchangeRateProvider()
        {

            var currencies = new List<Currency> { new Currency("USD") };
            var expected = new List<ExchangeRate> { new ExchangeRate(new Currency("CZK"), new Currency("USD"), 0.04m) };

            _mockExchangeRateProviderFactory.Setup(x => x.Create(ProviderType.api).GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime?>()))
                                        .ReturnsAsync(Enumerable.Empty<ExchangeRate>());

            _mockExchangeRateProviderFactory.Setup(x => x.Create(ProviderType.text).GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime>()))
                                         .ReturnsAsync(expected);

            var result = await _provider.GetExchangeRatesAsync(currencies, DateTime.Now);


            _mockExchangeRateProviderFactory.Verify(x => x.Create(ProviderType.text).GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime>()), Times.Once);
            result.ShouldBe(expected);
        }

        //I would add more tests but just showcasing how id approach em
    }

}
