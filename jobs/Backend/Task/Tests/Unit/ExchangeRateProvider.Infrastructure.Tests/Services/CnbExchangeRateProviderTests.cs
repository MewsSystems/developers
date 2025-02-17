namespace ExchangeRateProvider.Infrastructure.Tests.Services;

using System.Net;
using System.Text.Json;
using Domain.Entities;
using Domain.Options;
using Infrasctucture.Clients;
using ExchangeRateProvider.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

[TestFixture]
public class CnbExchangeRateProviderTests
{
    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.example.com")
        };

        _options = new CnbApiOptions
        {
            BaseUrl = "https://api.example.com",
            CacheDurationHours = 1
        };

        _optionsMock.SetupGet(o => o.Value).Returns(_options);

        _provider = new CnbExchangeRateProvider(
            new CnbCzClient(_httpClient),
            _optionsMock.Object,
            Mock.Of<ILogger<CnbExchangeRateProvider>>(),
            new MemoryCache(new MemoryCacheOptions()));
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }

    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new();
    private readonly Mock<IOptions<CnbApiOptions>> _optionsMock = new();
    private HttpClient _httpClient = new();
    private CnbApiOptions? _options;
    private CnbExchangeRateProvider _provider;

    [Test]
    public async Task GetExchangeRatesAsync_ValidRequest_ReturnsExpectedResults()
    {
        // Arrange
        var expectedResponse = new
        {
            Rates = new List<ExRateDailyRest>
            {
                new() { CurrencyCode = "USD", Rate = 22.5 },
                new() { CurrencyCode = "EUR", Rate = 25.3 }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(expectedResponse);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Get &&
                    r.RequestUri!.ToString().Contains("daily")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        // Act
        var result = await _provider.GetExchangeRatesAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.SourceCurrency.Code == "USD" && r.Value == 22.5);
        result.Should().Contain(r => r.SourceCurrency.Code == "EUR" && r.Value == 25.3);
    }

    [Test]
    public async Task GetExchangeRatesAsync_ShouldReturnRatesFromCache_WhenCacheIsAvailable()
    {
        // Arrange
        var cachedRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.5),
            new(new Currency("EUR"), new Currency("CZK"), 25.3)
        };

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        memoryCache.Set("ExchangeRates", cachedRates, TimeSpan.FromHours(1));

        var providerWithCache = new CnbExchangeRateProvider(
            new CnbCzClient(_httpClient),
            _optionsMock.Object,
            Mock.Of<ILogger<CnbExchangeRateProvider>>(),
            memoryCache);

        // Act
        var result = await providerWithCache.GetExchangeRatesAsync();

        // Assert
        result.Should().BeEquivalentTo(cachedRates);
    }

    [Test]
    public async Task GetExchangeRatesAsync_ShouldReturnEmptyList_WhenApiFails()
    {
        // Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("API error"));

        // Act
        var result = await _provider.GetExchangeRatesAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
