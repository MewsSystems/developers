using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Test.utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsDefinedRates()
        {
            // Arrange
            var apiResponse = new ExchangeRateApiResponse
            {
                Rates = new List<RateDTO>
                {
                    new RateDTO { CurrencyCode = "USD", RateValue = 23.341m, Amount = 1 },
                    new RateDTO { CurrencyCode = "EUR", RateValue = 24.945m, Amount = 1 }
                }
            };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.cnb.cz/*")
                .Respond("application/json", JsonConvert.SerializeObject(apiResponse));

            var httpClient = mockHttp.ToHttpClient();
            var clientWrapper = new HttpClientWrapper(httpClient);
            var logger = new NullLogger<ExchangeRateProvider>();
            var configuration = TestConfigurationHelper.BuildConfiguration();

            var parser = new CnbExchangeRateParser();
            var provider = new ExchangeRateProvider(clientWrapper, parser, configuration, logger);

            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
            };

            // Act
            var rates = await provider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Equal(2, rates.Count());
            Assert.Contains(rates, r => r.TargetCurrency.Code == "USD" && r.Value == 23.341m);
            Assert.Contains(rates, r => r.TargetCurrency.Code == "EUR" && r.Value == 24.945m);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_HandlesExceptionAndLogsError()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.cnb.cz/*")
                .Throw(new HttpRequestException("Request failed"));

            var httpClient = mockHttp.ToHttpClient();
            var clientWrapper = new HttpClientWrapper(httpClient);
            var mockLogger = new Mock<ILogger<ExchangeRateProvider>>();
            var configuration = TestConfigurationHelper.BuildConfiguration();

            var parser = new CnbExchangeRateParser();
            var provider = new ExchangeRateProvider(clientWrapper, parser, configuration, mockLogger.Object);

            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
            };

            // Act
            var rates = await provider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Empty(rates);
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error fetching data: Request failed")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }
    }
}
