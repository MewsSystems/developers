using ExchangeRateUpdater.Cnb;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderApiTests
    {
        [Fact]
        public async Task GetApiDataAsync_ReturnsData_OnSuccess()
        {
            // Arrange
            var goodJson = "{" +
                "\"rates\":[{" +
                "\"validFor\":\"2025-06-25\"," +
                "\"order\":121," +
                "\"country\":\"Australia\"," +
                "\"currency\":\"dollar\"," +
                "\"amount\":1," +
                "\"currencyCode\":\"AUD\"," +
                "\"rate\":13.872}]}";
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(goodJson)
                });
            var httpClient = new HttpClient(handlerMock.Object);
            var config = new Mock<IExchangeRateProviderConfiguration>();
            config.SetupGet(c => c.Url).Returns("http://test/api");
            config.SetupGet(c => c.BaseCurrency).Returns("CZK");
            var provider = new TestProvider(config.Object, httpClient);

            // Act
            var result = await provider.GetApiDataAsync<TestApiResponse>("http://test/api");

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data.Rates);
            Assert.Equal("AUD", result.Data.Rates[0].CurrencyCode);
        }

        [Fact]
        public async Task GetApiDataAsync_ReturnsError_OnApiError()
        {
            // Arrange
            var errorJson = "{" +
                "\"description\":\"Internal error\"," +
                "\"endPoint\":\"/api\"," +
                "\"errorCode\":\"INTERNAL_SERVER_ERROR\"," +
                "\"happenedAt\":\"2025-06-26T10:37:28.547Z\"," +
                "\"messageId\":\"abc123\"}";
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(errorJson)
                });
            var httpClient = new HttpClient(handlerMock.Object);
            var config = new Mock<IExchangeRateProviderConfiguration>();
            config.SetupGet(c => c.Url).Returns("http://test/api");
            config.SetupGet(c => c.BaseCurrency).Returns("CZK");
            var provider = new TestProvider(config.Object, httpClient);

            // Act
            var result = await provider.GetApiDataAsync<TestApiResponse>("http://test/api");

            // Assert
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Equal("INTERNAL_SERVER_ERROR", result.Error.ErrorCode);
        }

        // Minimal stub for testing
        public class TestProvider : ExchangeRateProviderBase
        {
            public TestProvider(IExchangeRateProviderConfiguration config, HttpClient client) : base(config, client) { }
            protected override Task<TestApiResponse> FetchRawDataAsync<TestApiResponse>() => throw new NotImplementedException();
            protected override IEnumerable<ExchangeRate> MapToExchangeRates<T>(T rawData, IEnumerable<Currency> currencies) => throw new NotImplementedException();
        }

        public class TestApiResponse
        {
            [JsonPropertyName("rates")]
            public List<TestRate> Rates { get; set; } = new List<TestRate>();
        }
        public class TestRate
        {
            [JsonPropertyName("currencyCode")]
            public string CurrencyCode { get; set; } = string.Empty;
            [JsonPropertyName("rate")]
            public double Rate { get; set; }
        }
    }
}
