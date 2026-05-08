using ExchangeRateProvider.Infrastructure.ExternalServices.CZK;
using ExchangeRateProvider.Infrastructure.Policies;
using LazyCache;
using LazyCache.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Polly;
using Polly.Registry;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace ExchangeRateProvider.Infrastructure.Tests.Unit.ExternalServices.CZK;

public class CzkApiClientTests
{
    private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
    private readonly IAppCache _cache;
    private readonly FakeLogger<CzkApiClient> _logger;

    public CzkApiClientTests()
    {
        _policyRegistry = new PolicyRegistry
        {
            [PolicyNames.WaitAndRetry] = Policy.NoOpAsync<HttpResponseMessage>()
        };
        _cache = new CachingService(new MemoryCacheProvider(new MemoryCache(new MemoryCacheOptions())));
        _logger = new FakeLogger<CzkApiClient>();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithSuccessfulResponse_ReturnsDeserializedData()
    {
        // Arrange
        var response = new CzkExchangeRateResponse(new List<CzkRate>
        {
            new() { Amount = 1, CurrencyCode = "USD", Rate = 23.5m, Country = "USA", Currency = "Dollar", Order = 1, ValidFor = DateOnly.FromDateTime(DateTime.Today) }
        });
        var httpClient = CreateHttpClientWithResponse(HttpStatusCode.OK, response);
        var client = new CzkApiClient(httpClient, _policyRegistry, _cache, _logger);

        // Act
        var result = await client.GetExchangeRatesAsync(CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Rates.ShouldHaveSingleItem();
        result.Rates[0].CurrencyCode.ShouldBe("USD");
        
        var logs = _logger.Collector.GetSnapshot();
        logs.Count.ShouldBe(2);
        logs[0].Level.ShouldBe(LogLevel.Information);
        logs[0].Message.ShouldContain("Fetching exchange rates from CNB API");
        logs[1].Level.ShouldBe(LogLevel.Information);
        logs[1].Message.ShouldContain("Successfully fetched and cached exchange rates from CNB API");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_SecondCall_ReturnsCachedData()
    {
        // Arrange
        var response = new CzkExchangeRateResponse(new List<CzkRate>
        {
            new() { Amount = 1, CurrencyCode = "USD", Rate = 23.5m, Country = "USA", Currency = "Dollar", Order = 1, ValidFor = DateOnly.FromDateTime(DateTime.Today) }
        });
        
        var callCount = 0;
        var handler = new MockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }), () => callCount++);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.cnb.cz") };
        var client = new CzkApiClient(httpClient, _policyRegistry, _cache, _logger);

        // Act
        var result1 = await client.GetExchangeRatesAsync(CancellationToken.None);
        var result2 = await client.GetExchangeRatesAsync(CancellationToken.None);

        // Assert
        result1.ShouldNotBeNull();
        result2.ShouldNotBeNull();
        callCount.ShouldBe(1);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithHttpError_ThrowsHttpRequestException()
    {
        // Arrange
        var httpClient = CreateHttpClientWithResponse(HttpStatusCode.InternalServerError, string.Empty);
        var client = new CzkApiClient(httpClient, _policyRegistry, _cache, _logger);

        // Act & Assert
        await Should.ThrowAsync<HttpRequestException>(() => client.GetExchangeRatesAsync(CancellationToken.None));
        
        var logs = _logger.Collector.GetSnapshot();
        logs[0].Level.ShouldBe(LogLevel.Information);
        logs[0].Message.ShouldContain("Fetching exchange rates from CNB API");
    }

    private static HttpClient CreateHttpClientWithResponse(HttpStatusCode statusCode, object content)
    {
        var json = content is string s ? s : JsonSerializer.Serialize(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var handler = new MockHttpMessageHandler(statusCode, json);
        return new HttpClient(handler) { BaseAddress = new Uri("https://api.cnb.cz") };
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _content;
        private readonly Action? _onSend;

        public MockHttpMessageHandler(HttpStatusCode statusCode, string content, Action? onSend = null)
        {
            _statusCode = statusCode;
            _content = content;
            _onSend = onSend;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _onSend?.Invoke();
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = new StringContent(_content)
            });
        }
    }
}
