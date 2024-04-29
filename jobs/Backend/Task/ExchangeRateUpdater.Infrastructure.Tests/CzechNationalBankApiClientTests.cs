namespace ExchangeRateUpdater.Infrastructure.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using Moq.Protected;
    using Xunit;

    public class CzechNationalBankApiClientTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly IExchangeRateProviderRepository _apiClient;

        public CzechNationalBankApiClientTests()
        {
            Mock<IHttpClientFactory> httpClientFactoryMock = new();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://www.test.com"),

            };
            httpClientFactoryMock.Setup(x => x.CreateClient("CzechNationalBankApi")).Returns(httpClient);

            var defaultResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{ \"rates\": [ { \"currencyCode\": \"USD\", \"rate\": 20, \"amount\": 1 } ] }"),
            };
            SetupHttpMessageHandlerMock(defaultResponse);

            _apiClient = new CzechNationalBankApiClient(httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task GetCentralBankRates_ReturnsRates_WhenApiCallIsSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ 
        ""rates"": [
            {
                ""validFor"": ""2024-04-26"",
                ""order"": 82,
                ""country"": ""Australia"",
                ""currency"": ""dollar"",
                ""amount"": 1,
                ""currencyCode"": ""AUD"",
                ""rate"": 15.35
            },
            {
                ""validFor"": ""2024-04-26"",
                ""order"": 82,
                ""country"": ""Brazil"",
                ""currency"": ""real"",
                ""amount"": 1,
                ""currencyCode"": ""BRL"",
                ""rate"": 4.555
            }
        ]
    }")
            };

            SetupHttpMessageHandlerMock(httpResponse);

            // Act
            var result = await _apiClient.GetCentralBankRates(CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public async Task GetCentralBankRates_ReturnsError_WhenApiCallFails()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };

            SetupHttpMessageHandlerMock(httpResponse);

            // Act
            var result = await _apiClient.GetCentralBankRates(CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
        }

        [Fact]
        public async Task GetCentralBankRates_ReturnsError_WhenDeserializationFails()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Invalid JSON"),
            };

            SetupHttpMessageHandlerMock(httpResponse);

            // Act
            var result = await _apiClient.GetCentralBankRates(CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
        }

        private void SetupHttpMessageHandlerMock(HttpResponseMessage response)
        {
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);
        }
    }
}