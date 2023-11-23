using Moq;
using Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates;
using Mews.ExchangeRateProvider.Infrastructure.Utils;
using Mews.ExchangeRateProvider.Infrastructure.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using Moq.Protected;
using Newtonsoft.Json;

namespace Mews.ExchangeRateProvider.Infrastructure.Test.CNBClient
{
    public class CNBClientTests
    {
        [Fact]
        public async Task GetDailyRatesCNBAsync_OnSuccessfulResponse_ShouldReturnsRates()
        {
            // Arrange
            var date = "2023-11-15";
            var lang = "en";

            var options = new CNBClientOptions { CnbDailyRatesUrl = "http://someexample.com/api" };
            var httpClientFactoryMock = GetHttpClientFactoryMock(HttpStatusCode.OK, JsonConvert.SerializeObject(GetSampleResponseExchangeRates()));
            var loggerMock = new Mock<ILogger<Clients.CNBClient>>();

            var cnbClient = new Clients.CNBClient(Mock.Of<IOptions<CNBClientOptions>>(x => x.Value == options), httpClientFactoryMock.Object, loggerMock.Object);

            // Act
            var result = await cnbClient.GetDailyRatesCNBAsync(date, lang);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetDailyRatesCNBAsync_OnUnsuccessfulResponse_ShouldThrowException()
        {
            // Arrange
            var date = "2023-11-13";
            var lang = "en";

            var options = new CNBClientOptions { CnbDailyRatesUrl = "http://someexample.com/api" };
            var httpClientFactoryMock = GetHttpClientFactoryMock(HttpStatusCode.InternalServerError, string.Empty);
            var loggerMock = new Mock<ILogger<Clients.CNBClient>>();

            var cnbClient = new Clients.CNBClient(Mock.Of<IOptions<CNBClientOptions>>(x => x.Value == options), httpClientFactoryMock.Object, loggerMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await cnbClient.GetDailyRatesCNBAsync(date, lang));
        }

        private Mock<IHttpClientFactory> GetHttpClientFactoryMock(HttpStatusCode statusCode, string responseContent = null)
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(responseContent),
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://someexample.com"),
            };

            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(() => httpClient);

            return httpClientFactoryMock;
        }


        private static ResponseExchangeRates GetSampleResponseExchangeRates()
        {
            return new ResponseExchangeRates
            {
                Rates = new List<ResponseExchangeRate>
                {
                    new ResponseExchangeRate
                    {
                        ValidFor = "2023-11-15",
                        Order = 218,
                        Country = "Austrálie",
                        Currency = "dolar",
                        Amount = 1,
                        CurrencyCode = "AUD",
                        Rate = 14.599m
                    },
                }
            };
        }
    }
}
