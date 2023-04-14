using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using System.Net;
using System.Threading;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ExchangeRateUpdater.Configuration;
using System.Reflection.Metadata;

namespace ExchangeRateUpdater.Tests
{
    public class CnbExchangeRateFetcherTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly Mock<IOptions<CnbSettings>> _settingsMock;
        private readonly string _testData;

        public CnbExchangeRateFetcherTests()
        {
            _testData = FileHelper.ReadTextFromFile("Files/DailyTestData.txt");

            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClientFactory
                .Setup(_ => _.CreateClient(Constants.CnbHttpClientKey))
                .Returns(new HttpClient(new MockHttpMessageHandler(_testData))
                {
                    BaseAddress = new Uri("https://daily-url.com")
                });

            _settingsMock = new Mock<IOptions<CnbSettings>>();
            _settingsMock.Setup(s => s.Value).Returns(new CnbSettings() { DailyUrl = "daily-url" });
        }

        [Fact]
        public async Task FetchDailyExchangeRateData_WithDate_ReturnsCorrectData()
        {
            // Arrange
            var date = DateOnly.Parse("2023-04-12");
            var exchangeRateFetcher = new CnbExchangeRateFetcher(_settingsMock.Object, _httpClientFactory.Object);

            // Act
            var result = await exchangeRateFetcher.FetchDailyExchangeRateData(date);

            // Assert
            Assert.Equal(_testData, result);
        }

        [Fact]
        public async Task FetchDailyExchangeRateData_WithoutDate_ReturnsCorrectData()
        {
            // Arrange
            var exchangeRateFetcher = new CnbExchangeRateFetcher(_settingsMock.Object, _httpClientFactory.Object);

            // Act
            var result = await exchangeRateFetcher.FetchDailyExchangeRateData(null);

            // Assert
            Assert.Equal(_testData, result);
        }
    }
}