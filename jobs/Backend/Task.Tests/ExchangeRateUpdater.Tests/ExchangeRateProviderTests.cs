using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests : IDisposable
    {
        private readonly Mock<ILogger<ExchangeRateProvider>> _loggerMock;
        private readonly Mock<IExchangeRateProviderConfiguration> _configMock;
        private readonly Mock<HttpMessageHandler> _httpHandlerMock;
        private readonly HttpClient _httpClient;

        public ExchangeRateProviderTests()
        {
            _loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
            _configMock = new Mock<IExchangeRateProviderConfiguration>();
            _httpHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpHandlerMock.Object)
            {
                Timeout = TimeSpan.FromSeconds(1)
            };

            _configMock.SetupGet(c => c.Url).Returns("https://api.cnb.cz/cnbapi/exrates/daily");
            _configMock.SetupGet(c => c.BaseCurrency).Returns("CZK");
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WithValidCurrencies_ReturnsExchangeRates()
        {
            // Arrange
            var responseJson = "{" +
                "\"rates\":[" +
                    "{\"validFor\":\"2025-06-26\"," +
                    "\"order\":121," +
                    "\"country\":\"USA\"," +
                    "\"currency\":\"dollar\"," +
                    "\"amount\":1," +
                    "\"currencyCode\":\"USD\"," +
                    "\"rate\":23.50}," +
                    "{\"validFor\":\"2025-06-26\"," +
                    "\"order\":122," +
                    "\"country\":\"EMU\"," +
                    "\"currency\":\"euro\"," +
                    "\"amount\":1," +
                    "\"currencyCode\":\"EUR\"," +
                    "\"rate\":25.75}" +
                "]}";

            SetupHttpResponse(HttpStatusCode.OK, responseJson);

            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            var provider = new ExchangeRateProvider(_configMock.Object, _httpClient);

            // Act
            var result = await provider.GetExchangeRatesAsync<CnbApiResponse>(currencies);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            
            var usdRate = resultList.FirstOrDefault(r => r.SourceCurrency.Code == "USD");
            Assert.NotNull(usdRate);
            Assert.Equal("USD", usdRate.SourceCurrency.Code);
            Assert.Equal("CZK", usdRate.TargetCurrency.Code);
            Assert.Equal(23.50m, usdRate.Value);

            var eurRate = resultList.FirstOrDefault(r => r.SourceCurrency.Code == "EUR");
            Assert.NotNull(eurRate);
            Assert.Equal("EUR", eurRate.SourceCurrency.Code);
            Assert.Equal("CZK", eurRate.TargetCurrency.Code);
            Assert.Equal(25.75m, eurRate.Value);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WithEmptyCurrencies_ReturnsEmptyResult()
        {
            // Arrange
            var responseJson = "{\"rates\":[]}";
            SetupHttpResponse(HttpStatusCode.OK, responseJson);

            var currencies = new List<Currency>();
            var provider = new ExchangeRateProvider(_configMock.Object, _httpClient);

            // Act
            var result = await provider.GetExchangeRatesAsync<CnbApiResponse>(currencies);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WithNullCurrencies_ThrowsArgumentNullException()
        {
            // Arrange
            var provider = new ExchangeRateProvider(_configMock.Object, _httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                provider.GetExchangeRatesAsync<CnbApiResponse>(null));
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WithUnavailableCurrency_ReturnsOnlyAvailableRates()
        {
            // Arrange
            var responseJson = "{" +
                "\"rates\":[" +
                    "{\"validFor\":\"2025-06-26\"," +
                    "\"order\":121," +
                    "\"country\":\"USA\"," +
                    "\"currency\":\"dollar\"," +
                    "\"amount\":1," +
                    "\"currencyCode\":\"USD\"," +
                    "\"rate\":23.50}" +
                "]}";

            SetupHttpResponse(HttpStatusCode.OK, responseJson);

            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("XYZ") // Non-existent currency
            };

            var provider = new ExchangeRateProvider(_configMock.Object, _httpClient);

            // Act
            var result = await provider.GetExchangeRatesAsync<CnbApiResponse>(currencies);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Single(resultList);
            Assert.Equal("USD", resultList.First().SourceCurrency.Code);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WhenApiReturnsError_ThrowsException()
        {
            // Arrange
            var errorJson = "{" +
                "\"description\":\"API Error\"," +
                "\"endPoint\":\"/cnbapi/exrates/daily\"," +
                "\"errorCode\":\"INTERNAL_SERVER_ERROR\"," +
                "\"happenedAt\":\"2025-06-26T10:37:28.547Z\"," +
                "\"messageId\":\"abc123\"}";

            SetupHttpResponse(HttpStatusCode.InternalServerError, errorJson);

            var currencies = new List<Currency> { new Currency("USD") };
            var provider = new ExchangeRateProvider(_configMock.Object, _httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                provider.GetExchangeRatesAsync<CnbApiResponse>(currencies));
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WhenNetworkFails_ThrowsException()
        {
            // Arrange
            _httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"))
                .Verifiable();

            var currencies = new List<Currency> { new Currency("USD") };
            var provider = new ExchangeRateProvider(_configMock.Object, _httpClient);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                provider.GetExchangeRatesAsync<CnbApiResponse>(currencies));
            
            Assert.Contains("Network error", exception.Message);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WithInvalidJson_ThrowsException()
        {
            // Arrange
            var invalidJson = "{ invalid json structure";
            SetupHttpResponse(HttpStatusCode.OK, invalidJson);

            var currencies = new List<Currency> { new Currency("USD") };
            var provider = new ExchangeRateProvider(_configMock.Object, _httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                provider.GetExchangeRatesAsync<CnbApiResponse>(currencies));
        }

        private void SetupHttpResponse(HttpStatusCode statusCode, string content)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json")
            };

            _httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response)
                .Verifiable();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
