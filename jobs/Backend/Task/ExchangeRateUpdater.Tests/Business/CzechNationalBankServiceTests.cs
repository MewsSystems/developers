using ExchangeRateUpdater.Business;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

namespace ExchangeRateUpdater.Tests.Business
{
    public class CzechNationalBankServiceTests
    {
        private Mock<ILogger<CzechNationalBankService>> _mockLogger;
        private Mock<IOptions<CzechNationalBankOptions>> _mockOptions;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<CzechNationalBankService>>();
            _mockOptions = new Mock<IOptions<CzechNationalBankOptions>>();
        }

        [Test]
        public async Task GetLiveRatesAsync_CanNotLoadOptions_ReturnsNull()
        {
            // Arrange
            var mockHttpClient = new Mock<HttpClient>();
            _mockOptions.Setup(x => x.Value).Returns((CzechNationalBankOptions)null);

            var service = new CzechNationalBankService(_mockLogger.Object, mockHttpClient.Object, _mockOptions.Object);

            // Act
            var result = await service.GetLiveRatesAsync();

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetLiveRatesAsync_NoEndpointSpecified_ReturnsNull()
        {
            // Arrange
            var mockHttpClient = new Mock<HttpClient>();
            _mockOptions.Setup(x => x.Value).Returns(new CzechNationalBankOptions { Endpoint = string.Empty});

            var service = new CzechNationalBankService(_mockLogger.Object, mockHttpClient.Object, _mockOptions.Object);

            // Act
            var result = await service.GetLiveRatesAsync();

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public async Task GetLiveRatesAsync_ApiReturnsErrorCode_ReturnsNull(HttpStatusCode statusCode)
        {
            // Arrange
            _mockOptions.Setup(x => x.Value).Returns(GetValidOptions());
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = statusCode
            };
            
            // With HttpClients, the real work is done by the Message Handler,
            // so this is the object we need to manipulate when sending a HTTP request
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("http://localhost") };
            var service = new CzechNationalBankService(_mockLogger.Object, httpClient, _mockOptions.Object);

            // Act
            var result = await service.GetLiveRatesAsync();

            // Assert
            Assert.That(result, Is.Null);
            mockHttpMessageHandler
                .Protected()
                .Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetLiveRatesAsync_ApiReturnsOk_ReturnsList()
        {
            // Arrange
            _mockOptions.Setup(x => x.Value).Returns(GetValidOptions());
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var content = GetValidApiResponse();
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content)
            };

            // With HttpClients, the real work is done by the Message Handler,
            // so this is the object we need to manipulate when sending a HTTP request
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("http://localhost") };
            var service = new CzechNationalBankService(_mockLogger.Object, httpClient, _mockOptions.Object);

            // Act
            var result = await service.GetLiveRatesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result, Is.TypeOf<List<ThirdPartyExchangeRate>>());
            mockHttpMessageHandler
                .Protected()
                .Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get), ItExpr.IsAny<CancellationToken>());
        }

        #region Test data helpers

        private CzechNationalBankOptions GetValidOptions()
        {
            return new CzechNationalBankOptions
            {
                BaseAddress = "http://localhost/",
                Endpoint = "daily"
            };
        }

        private string GetValidApiResponse ()
        {
            return @"
            {
	            ""rates"": [
		            {
			            ""validFor"": ""2023-05-17"",
			            ""order"": 94,
			            ""country"": ""Australia"",
			            ""currency"": ""dollar"",
			            ""amount"": 1,
			            ""currencyCode"": ""AUD"",
			            ""rate"": 14.529
		            },
		            {
			            ""validFor"": ""2023-05-17"",
			            ""order"": 94,
			            ""country"": ""Brazil"",
			            ""currency"": ""real"",
			            ""amount"": 1,
			            ""currencyCode"": ""BRL"",
			            ""rate"": 4.403
		            },
		            {
			            ""validFor"": ""2023-05-17"",
			            ""order"": 94,
			            ""country"": ""Bulgaria"",
			            ""currency"": ""lev"",
			            ""amount"": 1,
			            ""currencyCode"": ""BGN"",
			            ""rate"": 12.083
		            }]
            }
            ";
        }

        #endregion
    }
}
