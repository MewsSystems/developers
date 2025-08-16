using ExchangeRateUpdater.Domain.Constants;
using ExchangeRateUpdater.Domain.Models.Response;
using ExchangeRateUpdater.Infrastructure.Connectors;

using Moq;
using Moq.Protected;

using System.Net;

namespace ExchangeRateUpdater.UnitTests.Infrastructure.Connectors
{
    public class BaseCzechNationalBankConnectorTests
    {
        protected Mock<IHttpClientFactory> mockHttpClientFactory;
        protected readonly HttpClient httpClient;
        protected readonly Mock<HttpMessageHandler> mockHttpMessageHandler;

        public BaseCzechNationalBankConnectorTests()
        {
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.cnb.cz/cnbapi/")
            };
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(clientFactory => clientFactory.CreateClient(BankConstants.CzechNationalBank.HttpClientIdentifier)).Returns(httpClient);
        }

        public CzechNationalBankConnector GetConnector()
        {
            return new CzechNationalBankConnector(mockHttpClientFactory.Object);
        }

        public void MockExchangeResponse(ExchangeRatesResponse? exchangeRatesResponse, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(exchangeRatesResponse),
                    Encoding.UTF8,
                    "application/json")
            };

            MockHttpMessageHandler(response);
        }

        private void MockHttpMessageHandler(HttpResponseMessage response)
        {
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }
    }
}
