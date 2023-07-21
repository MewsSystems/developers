using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Providers;
using Microsoft.Extensions.Configuration;
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
    public class TextExchangeRateProviderTests
    {
        private Mock<IHttpClientService> _mockHttpClientService;
        private Mock<IExchangeRateParser> _mockExchangeRateParser;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ILogger<TextExchangeRateProvider>> _mockLogger;

        private TextExchangeRateProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _mockHttpClientService = new Mock<IHttpClientService>();
            _mockExchangeRateParser = new Mock<IExchangeRateParser>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<TextExchangeRateProvider>>();

            _provider = new TextExchangeRateProvider(_mockHttpClientService.Object, _mockExchangeRateParser.Object, _mockConfiguration.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetExchangeRatesAsync_ShouldCallDependenciesCorrectly()
        {

            var currencies = new List<Currency> { new Currency("USD") };
            var expected = new List<ExchangeRate> { new ExchangeRate(new Currency("CZK"), new Currency("USD"), 0.04m) };

            _mockConfiguration.Setup(x => x["ExchangeRateUrl"]).Returns("some_url");
            _mockHttpClientService.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync("response_string");
            _mockExchangeRateParser.Setup(x => x.ParseRatesFromText(It.IsAny<IEnumerable<Currency>>(), It.IsAny<string[]>())).Returns(expected);

       
            var result = await _provider.GetExchangeRatesAsync(currencies, null);

            _mockHttpClientService.Verify(x => x.GetStringAsync(It.IsAny<string>()), Times.Once);
            _mockExchangeRateParser.Verify(x => x.ParseRatesFromText(It.IsAny<IEnumerable<Currency>>(), It.IsAny<string[]>()), Times.Once);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetExchangeRatesAsync_WhenHttpClientThrowsException_ShouldLogError()
        {
          
            var currencies = new List<Currency> { new Currency("USD") };

            _mockConfiguration.Setup(x => x["ExchangeRateUrl"]).Returns("some_url");
            _mockHttpClientService.Setup(x => x.GetStringAsync(It.IsAny<string>())).ThrowsAsync(new Exception());

         
            Assert.ThrowsAsync<Exception>(async () => await _provider.GetExchangeRatesAsync(currencies, null));
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
        //I would add more tests but just showcasing how id approach em
    }
}
