using ExchangeRateUpdater.Infrastucture.Data.API.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ExchangeRateUpdater.Tests.UnitTests.Infrastructure
{
    public sealed class ExternalAPIServiceTests
    {
        [Fact]
        public async Task GetExternalAPIData_ValidInput_ReturnsData()
        {
            // Arrange
            var httpClient = new HttpClient();
            var httpClientMock = new Mock<IHttpClientFactory>();
            httpClientMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(x => x["ExternalApiSettings:BaseUrlExchangeRatesDaily"]).Returns("https://api.cnb.cz/cnbapi/exrates/daily");
            configurationMock.SetupGet(x => x["ExternalApiSettings:SourceCurrency"]).Returns("CZK");
            configurationMock.SetupGet(x => x["ExternalApiSettings:Language"]).Returns("EN");

            var sut = new ExchangeRatesDailyAPIService(httpClientMock.Object, configurationMock.Object);

            // Act
            var result = await sut.GetExternalDataAsync();

            // Assert
            Assert.NotNull(result);
        }
    }
}