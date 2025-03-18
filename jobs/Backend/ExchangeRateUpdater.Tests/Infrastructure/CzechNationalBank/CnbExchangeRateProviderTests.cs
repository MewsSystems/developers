using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests.Infrastructure.CzechNationalBank
{
    public class CnbExchangeRateProviderTests
    {
        private CnbExchangeRateResponse GetFakeResponse()
        {
            return new CnbExchangeRateResponse
            {
                Rates = new List<CnbRate>
                {
                    new CnbRate
                    {
                        ValidFor = "2025-03-18",
                        Amount = 1,
                        CurrencyCode = "USD",
                        Rate = 23.5m
                    },
                    new CnbRate
                    {
                        ValidFor = "2025-03-18",
                        Amount = 100,
                        CurrencyCode = "JPY",
                        Rate = 1543m
                    }
                }
            };
        }

        private string GetFakeJsonResponse()
        {
            var fakeResponse = GetFakeResponse();
            return JsonSerializer.Serialize(fakeResponse);
        }

        private CnbExchangeRateProvider CreateProvider(HttpMessageHandler handler, IMemoryCache cache, TimeSpan cacheDuration)
        {
            var httpClient = new HttpClient(handler);
            var loggerMock = new Mock<ILogger<CnbExchangeRateProvider>>();
            return new CnbExchangeRateProvider(httpClient, cache, cacheDuration, loggerMock.Object);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_GoodRequestGoodResponse_ReturnsMappedRates()
        {
            // Arrange
            string json = GetFakeJsonResponse();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.IsAny<HttpRequestMessage>(),
                           ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(json)
                       })
                       .Verifiable();

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            TimeSpan cacheDuration = TimeSpan.FromMinutes(30);
            var provider = CreateProvider(handlerMock.Object, memoryCache, cacheDuration);
            var requestedCurrencies = new List<Currency> { new Currency("USD"), new Currency("JPY") };

            // Act
            var result = await provider.GetExchangeRatesAsync(requestedCurrencies);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.SourceCurrency.Code == "USD" && Math.Abs(r.Value - 23.5m) < 0.001m);
            Assert.Contains(result, r => r.SourceCurrency.Code == "JPY" && Math.Abs(r.Value - 15.43m) < 0.001m);
            handlerMock.Protected().Verify("SendAsync", Times.Once(),
                                           ItExpr.IsAny<HttpRequestMessage>(),
                                           ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ApiError_ThrowsHttpRequestException()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.IsAny<HttpRequestMessage>(),
                           ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.InternalServerError,
                           Content = new StringContent("Server error")
                       })
                       .Verifiable();

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            TimeSpan cacheDuration = TimeSpan.FromMinutes(30);
            var provider = CreateProvider(handlerMock.Object, memoryCache, cacheDuration);
            var requestedCurrencies = new List<Currency> { new Currency("USD") };

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => provider.GetExchangeRatesAsync(requestedCurrencies));
        }

        [Fact]
        public async Task GetExchangeRatesAsync_CachingWithDifferentCurrencies_UsesCache()
        {
            // Arrange
            string json = GetFakeJsonResponse();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.IsAny<HttpRequestMessage>(),
                           ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(json)
                       })
                       .Verifiable();

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            TimeSpan cacheDuration = TimeSpan.FromMinutes(30);
            var provider = CreateProvider(handlerMock.Object, memoryCache, cacheDuration);

            var requestedCurrencies1 = new List<Currency> { new Currency("USD"), new Currency("JPY") };
            var result1 = await provider.GetExchangeRatesAsync(requestedCurrencies1);

            var requestedCurrencies2 = new List<Currency> { new Currency("USD") };
            var result2 = await provider.GetExchangeRatesAsync(requestedCurrencies2);

            // Assert
            Assert.Equal(2, result1.Count());
            Assert.Single(result2);

            handlerMock.Protected().Verify("SendAsync", Times.Once(),
                                           ItExpr.IsAny<HttpRequestMessage>(),
                                           ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetExchangeRatesAsync_RequestOnlyFakeCurrencies_ReturnsEmpty()
        {
            // Arrange
            string json = GetFakeJsonResponse();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.IsAny<HttpRequestMessage>(),
                           ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(json)
                       })
                       .Verifiable();

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            TimeSpan cacheDuration = TimeSpan.FromMinutes(30);
            var provider = CreateProvider(handlerMock.Object, memoryCache, cacheDuration);

            var requestedCurrencies = new List<Currency> { new Currency("ABC"), new Currency("DEF") };

            // Act
            var result = await provider.GetExchangeRatesAsync(requestedCurrencies);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_NullCurrencies_ReturnsEmpty()
        {
            // Arrange
            string json = GetFakeJsonResponse();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.IsAny<HttpRequestMessage>(),
                           ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(json)
                       })
                       .Verifiable();

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            TimeSpan cacheDuration = TimeSpan.FromMinutes(30);
            var provider = CreateProvider(handlerMock.Object, memoryCache, cacheDuration);

            // Act
            var result = await provider.GetExchangeRatesAsync(null);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_InvalidJsonResponse_ThrowsJsonException()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.IsAny<HttpRequestMessage>(),
                           ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent("Invalid JSON")
                       })
                       .Verifiable();

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            TimeSpan cacheDuration = TimeSpan.FromMinutes(30);
            var provider = CreateProvider(handlerMock.Object, memoryCache, cacheDuration);
            var requestedCurrencies = new List<Currency> { new Currency("USD") };

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(() => provider.GetExchangeRatesAsync(requestedCurrencies));
        }

        [Fact]
        public async Task GetExchangeRatesAsync_CacheExpiry_FetchesNewData()
        {
            // Arrange
            string json = GetFakeJsonResponse();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.IsAny<HttpRequestMessage>(),
                           ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(new HttpResponseMessage
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(json)
                       })
                       .Verifiable();

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            TimeSpan cacheDuration = TimeSpan.FromSeconds(1);
            var provider = CreateProvider(handlerMock.Object, memoryCache, cacheDuration);
            var requestedCurrencies = new List<Currency> { new Currency("USD"), new Currency("JPY") };

            // Act
            var result1 = await provider.GetExchangeRatesAsync(requestedCurrencies);
            await Task.Delay(2000);
            var result2 = await provider.GetExchangeRatesAsync(requestedCurrencies);

            // Assert
            Assert.Equal(2, result1.Count());
            Assert.Equal(2, result2.Count());
            handlerMock.Protected().Verify("SendAsync", Times.Exactly(2),
                                           ItExpr.IsAny<HttpRequestMessage>(),
                                           ItExpr.IsAny<CancellationToken>());
        }
    }
}
