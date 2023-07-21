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
        private Mock<IApiExchangeRateProvider> _mockApiExchangeRateProvider;
        private Mock<ITextExchangeRateProvider> _mockTextExchangeRateProvider;
        private ExchangeRateProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<ExchangeRateProvider>>();
            _mockApiExchangeRateProvider = new Mock<IApiExchangeRateProvider>();
            _mockTextExchangeRateProvider = new Mock<ITextExchangeRateProvider>();

            _provider = new ExchangeRateProvider(_mockApiExchangeRateProvider.Object, _mockTextExchangeRateProvider.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetExchangeRatesAsync_WhenApiReturnsData_ShouldNotCallTextExchangeRateProvider()
        {
            
            var currencies = new List<Currency> { new Currency("USD") };
            var expected = new List<ExchangeRate> { new ExchangeRate(new Currency("CZK"), new Currency("USD"), 0.04m) };

            _mockApiExchangeRateProvider.Setup(x => x.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime?>()))
                                        .ReturnsAsync(expected);

          
            var result = await _provider.GetExchangeRatesAsync(currencies, DateTime.Now);

        
            _mockTextExchangeRateProvider.Verify(x => x.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime>()), Times.Never);
            result.ShouldBe(expected);
        }

        [Test]
        public async Task GetExchangeRatesAsync_WhenApiReturnsNoData_ShouldCallTextExchangeRateProvider()
        {

            var currencies = new List<Currency> { new Currency("USD") };
            var expected = new List<ExchangeRate> { new ExchangeRate(new Currency("CZK"), new Currency("USD"), 0.04m) };

            _mockApiExchangeRateProvider.Setup(x => x.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime?>()))
                                        .ReturnsAsync(Enumerable.Empty<ExchangeRate>());

            _mockTextExchangeRateProvider.Setup(x => x.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime>()))
                                         .ReturnsAsync(expected);

            var result = await _provider.GetExchangeRatesAsync(currencies, DateTime.Now);

           
            _mockTextExchangeRateProvider.Verify(x => x.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime>()), Times.Once);
            result.ShouldBe(expected);
        }

        //I would add more tests but just showcasing how id approach em
    }

}
