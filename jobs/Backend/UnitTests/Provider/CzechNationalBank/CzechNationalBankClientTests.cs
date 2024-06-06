using ExchangeRateUpdater.ExchangeRate.Constant;
using ExchangeRateUpdater.ExchangeRate.Exception;
using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank;
using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank.Model;
using ExchangeRateUpdater.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Tests.ExchangeRate.Provider.CzechNationalBank
{
    public class CzechNationalBankClientTests
    {
        [Fact]
        public async Task GetDailyExchangeRates_ReturnsExchangeRates_Successfully()
        {
            // Arrange
            var expectedResponse = new CzechNationalBankDailyExchangeRateResponse
            {
                ExchangeRates =
                [
                    new CzechNationalBankExchangeRate { Currency = "USD", Rate = 1.23m }
                ]
            };

            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(JsonUtil.Serialize(expectedResponse));

            var configMock = new Mock<IOptionsMonitor<CzechNationalBankConfig>>();
            configMock.SetupGet(x => x.CurrentValue).Returns(new CzechNationalBankConfig { BaseUrl = "https://example.com", DailyExchangeRateEndpoint = "api/rates" });

            var loggerMock = new Mock<ILogger<CzechNationalBankClient>>();
            var client = new CzechNationalBankClient(httpServiceMock.Object, configMock.Object, loggerMock.Object);
            var request = new FetchCzechNationalBankDailyExchangeRateRequest(DateOnly.FromDateTime(DateTime.Today), Language.EN);


            // Act
            var response = await client.GetDailyExchangeRates(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Single(response.ExchangeRates);
            Assert.Equal("USD", response.ExchangeRates.First().Currency);
            Assert.Equal(1.23m, response.ExchangeRates.First().Rate);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ThrowsException_ForFailedHttpRequest()
        {
            // Arrange
            var configMock = new Mock<IOptionsMonitor<CzechNationalBankConfig>>();
            configMock.SetupGet(x => x.CurrentValue).Returns(new CzechNationalBankConfig { BaseUrl = "https://example.com", DailyExchangeRateEndpoint = "api/rates" });

            var loggerMock = new Mock<ILogger<CzechNationalBankClient>>();

            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Throws<HttpRequestException>();

            var client = new CzechNationalBankClient(httpServiceMock.Object, configMock.Object, loggerMock.Object);
            var request = new FetchCzechNationalBankDailyExchangeRateRequest(DateOnly.FromDateTime(DateTime.Today), Language.EN);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ExchangeRateUpdaterException>(() => client.GetDailyExchangeRates(request, CancellationToken.None));
            Assert.Contains("Failed to fetch daily exchange rates from the provider", exception.Message);
        }
    }
}
