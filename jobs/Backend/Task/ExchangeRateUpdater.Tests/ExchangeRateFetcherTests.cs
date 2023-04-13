using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using System.Net;
using System.Threading;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateFetcherTests
    {
        [Fact]
        public async Task FetchExchangeRateData_WithDate_ReturnsCorrectData()
        {
            // Arrange
            var testData = FileHelper.ReadTextFromFile("TestData.txt");
            var httpClient = GetHttpClientWithResponse(testData);
            var cnbSettings = GetCNBSettings("http://www.testurl.com");
            var mockClock = new MockClock(DateOnly.Parse("2023-04-12"));

            var exchangeRateFetcher = new ExchangeRateFetcher(httpClient, cnbSettings, mockClock);

            // Act
            var result = await exchangeRateFetcher.FetchExchangeRateData();

            // Assert
            Assert.Equal(testData, result);
        }

        private HttpClient GetHttpClientWithResponse(string response)
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response)
                });

            return new HttpClient(httpMessageHandler.Object);
        }

        private IOptions<CNBSettings> GetCNBSettings(string url)
        {
            var cnbSettings = new CNBSettings { Url = url };
            return Options.Create(cnbSettings);
        }
    }
}