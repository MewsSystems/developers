using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterTests
{
    [TestFixture]
    public class ExchangeRateServiceTests
    {
        private Mock<ILogger<IExchangeRateService>> _mockLogger;
        private Mock<IExchangeRateProvider> _mockExchangeRateProvider;
        private Mock<ICurrencyLoader> _mockCurrencyLoader;
        private Mock<IOutputService> _mockOutputService;

        private ExchangeRateService _exchangeRateService;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<IExchangeRateService>>();
            _mockExchangeRateProvider = new Mock<IExchangeRateProvider>();
            _mockCurrencyLoader = new Mock<ICurrencyLoader>();
            _mockOutputService = new Mock<IOutputService>();

            _exchangeRateService = new ExchangeRateService(_mockExchangeRateProvider.Object, _mockCurrencyLoader.Object, _mockLogger.Object, _mockOutputService.Object);
        }

        [Test]
        public async Task ExecuteAsync_CallsGetExchangeRatesAsyncWithCurrentDate()
        {

            var currencies = new List<Currency> { new Currency("USD") };
            var expected = new List<ExchangeRate> { new ExchangeRate(new Currency("CZK"), new Currency("USD"), 0.04m) };

            _mockCurrencyLoader.Setup(x => x.LoadCurrencies()).Returns(currencies);
            _mockExchangeRateProvider.Setup(x => x.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<DateTime>()))
                                     .ReturnsAsync(expected);

 
            await _exchangeRateService.ExecuteAsync();

      
            _mockExchangeRateProvider.Verify(x => x.GetExchangeRatesAsync(currencies, It.IsAny<DateTime>()), Times.Once);
        }
        //I would add more tests but just showcasing how id approach em


    }
}
