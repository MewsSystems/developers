using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderUnitTests
    {
        [Fact]
        public async Task GetExchangeRates_ReturnsCorrectData()
        {
            var loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["ExchangeRateApi:BaseUrl"]).Returns("http://testurl.com");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{ \"rates\": [ { \"currencyCode\": \"USD\", \"rate\": 25.50 }, { \"currencyCode\": \"EUR\", \"rate\": 27.0 } ] }")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var provider = new ExchangeRateProvider(loggerMock.Object, configurationMock.Object, httpClient);
            var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
            var rates = await provider.GetExchangeRates(currencies);
            
            Assert.NotNull(rates);
            Assert.Contains(rates, r => r.TargetCurrency.Code == "USD" || r.TargetCurrency.Code == "EUR");
        }
        
         [Fact]
        public async Task GetExchangeRates_WhenApiReturnsError_ThrowsHttpRequestException()
        {
            var loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["ExchangeRateApi:BaseUrl"]).Returns("http://testurl.com");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var provider = new ExchangeRateProvider(loggerMock.Object, configurationMock.Object, httpClient);

            var currencies = new List<Currency> { new Currency("USD") };

            var exception = await Assert.ThrowsAsync<Exception>(() => provider.GetExchangeRates(currencies));
            Assert.Contains("Error fetching data from Czech National Bank", exception.Message);
        }

        [Fact]
        public async Task GetExchangeRates_WhenNetworkFails_ThrowsHttpRequestException()
        {
            var loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["ExchangeRateApi:BaseUrl"]).Returns("http://testurl.com");

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var provider = new ExchangeRateProvider(loggerMock.Object, configurationMock.Object, httpClient);

            var currencies = new List<Currency> { new Currency("USD") };

            var exception = await Assert.ThrowsAsync<Exception>(() => provider.GetExchangeRates(currencies));
            Assert.Contains("Error fetching data from Czech National Bank", exception.Message);
        }
    }
}