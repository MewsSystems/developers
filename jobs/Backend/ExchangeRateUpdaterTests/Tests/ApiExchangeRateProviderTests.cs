using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Providers;
using System.Linq;
using ExchangeRateUpdater.Helpers;

namespace ExchangeRateUpdater.Tests
{
    public class ApiExchangeRateProviderTests
    {
        private Mock<IHttpClientService> _mockHttpClientService;
        private Mock<IExchangeRateParser> _mockExchangeRateParser; 
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ILogger<ApiExchangeRateProvider>> _mockLogger;
        private ApiExchangeRateProvider _provider;


        [SetUp]
        public void Setup()
        {
            _mockHttpClientService = new Mock<IHttpClientService>();
            _mockExchangeRateParser = new Mock<IExchangeRateParser>(); 
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<ApiExchangeRateProvider>>();

            _provider = new ApiExchangeRateProvider(_mockHttpClientService.Object, _mockExchangeRateParser.Object, _mockConfiguration.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetExchangeRatesAsync_Should_Call_HttpClientService_GetStringAsync_Once()
        {

            string apiUrl = "http://test.url";
            _mockConfiguration.Setup(c => c["ApiUrl"]).Returns(apiUrl);
            _mockHttpClientService.Setup(h => h.GetStringAsync(apiUrl)).ReturnsAsync("{}");

            var result = await _provider.GetExchangeRatesAsync(new List<Currency>() { new Currency("USD") }, DateTime.Now);

            _mockHttpClientService.Verify(h => h.GetStringAsync(apiUrl), Times.Once);
        }

       //I would add more tests but just showcasing how id approach em
    }
}
