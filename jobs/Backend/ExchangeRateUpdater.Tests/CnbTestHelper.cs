using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Core.Clients;
using ExchangeRateUpdater.Core.Services;

namespace ExchangeRateUpdater.Tests
{
    public static class CnbTestHelper
    {
        /// <summary>
        /// Creates a CnbExchangeRateProvider with a mocked CnbExchangeRateClient
        /// that returns predefined JSON and status code.
        /// </summary>
        /// <returns>
        /// Tuple containing the provider, logger mock, and HTTP handler mock.
        /// </returns>
        public static (CnbExchangeRateProvider Provider, Mock<ILogger<CnbExchangeRateProvider>> Logger, Mock<HttpMessageHandler> Handler)
            CreateProviderAndDependencies(string jsonResponse, HttpStatusCode statusCode)
        {
            // Mock the HttpMessageHandler
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                })
                .Verifiable();

            // Create an HttpClient with that mock
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new System.Uri("https://api.cnb.cz")
            };

            // Mock the logger for the client
            var clientLogger = new Mock<ILogger<CnbExchangeRateClient>>();

            // Create the real CnbExchangeRateClient with the mocked HttpClient
            var cnbClient = new CnbExchangeRateClient(httpClient, clientLogger.Object);

            // Mock the logger for the provider
            var providerLogger = new Mock<ILogger<CnbExchangeRateProvider>>();

            // Create the real provider with the real cnbClient
            var provider = new CnbExchangeRateProvider(providerLogger.Object, cnbClient);

            return (provider, providerLogger, handlerMock);
        }
    }
}
