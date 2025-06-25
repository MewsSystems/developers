using ExchangeRateUpdater.Cnb;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        private static ExchangeRateProvider CreateProvider(HttpMessageHandler handler)
        {
            var httpClient = new HttpClient(handler);
            var config = new TestConfig { Url = "http://test/api", BaseCurrency = "CZK" };
            return new ExchangeRateProvider(config, httpClient);
        }

        [Fact]
        public async Task FetchRawDataAsync_ReturnsRates_OnSuccess()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new CnbApiResponse { Rates = new List<CnbRateDto> { new CnbRateDto { CurrencyCode = "USD", Amount = 1, Rate = 25.0m } } })
            };
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var provider = CreateProvider(handler.Object);
            var result = await provider.GetExchangeRatesAsync<CnbApiResponse>(new[] { new Currency("USD") });
            Assert.Single(result);
            Assert.Equal("USD", result.First().SourceCurrency.Code);
        }

        [Fact]
        public async Task FetchRawDataAsync_Throws_OnApiError()
        {
            var errorJson = "{\"description\":\"Bad request\",\"errorCode\":\"BAD_REQUEST\"}";
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(errorJson, System.Text.Encoding.UTF8, "application/json")
            };
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var provider = CreateProvider(handler.Object);
            
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
                await provider.GetExchangeRatesAsync<CnbApiResponse>(new[] { new Currency("USD") })
            );
        }

        private class TestConfig : IExchangeRateProviderConfiguration
        {
            public string Url { get; set; } = string.Empty;
            public string BaseCurrency { get; set; } = string.Empty;
        }
    }
}
