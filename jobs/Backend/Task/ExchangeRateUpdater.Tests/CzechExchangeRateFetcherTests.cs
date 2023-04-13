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

namespace ExchangeRateUpdater.Tests
{
    public class CzechExchangeRateFetcherTests
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<IOptions<CNBSettings>> _settingsMock;
        private readonly string _testData;

        public CzechExchangeRateFetcherTests()
        {
            _testData = FileHelper.ReadTextFromFile("Files/DailyTestData.txt");
            _httpClient = new HttpClient(new MockHttpMessageHandler(_testData))
            {
                BaseAddress = new Uri("https://daily-url.com")
            };

            _settingsMock = new Mock<IOptions<CNBSettings>>();
            _settingsMock.Setup(s => s.Value).Returns(new CNBSettings() { DailyUrl = "daily-url" });
        }

        [Fact]
        public async Task FetchDailyExchangeRateData_WithDate_ReturnsCorrectData()
        {
            // Arrange
            var date = DateOnly.Parse("2023-04-12");
            var exchangeRateFetcher = new CzechExchangeRateFetcher(_httpClient, _settingsMock.Object);

            // Act
            var result = await exchangeRateFetcher.FetchDailyExchangeRateData(date);

            // Assert
            Assert.Equal(_testData, result);
        }

        [Fact]
        public async Task FetchDailyExchangeRateData_WithoutDate_ReturnsCorrectData()
        {
            // Arrange
            var exchangeRateFetcher = new CzechExchangeRateFetcher(_httpClient, _settingsMock.Object);

            // Act
            var result = await exchangeRateFetcher.FetchDailyExchangeRateData(null);

            // Assert
            Assert.Equal(_testData, result);
        }
    }
}