using ExchangeRateProvider.Domain.Entities;
using ExchangeRateProvider.Infrastructure.ExchangeRateProviders.Cnb;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ExchangeRateProvider.Infrastructure.Tests.ExchangeRateProviders.Cnb;

public class CnbExchangeRateProviderTests : IDisposable
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly CnbApiOptions _cnbApiOptions;
    private readonly CnbExchangeRateProvider _provider;

    public CnbExchangeRateProviderTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockOptions = new Mock<IOptions<CnbApiOptions>>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        _cnbApiOptions = new CnbApiOptions
        {
            HttpClientName = "CnbApiClient",
            BaseUrl = "https://api.cnb.cz",
            DailyExchangeRatesEndpoint = "/cnbapi/exrates/daily"
        };

        mockOptions.Setup(x => x.Value).Returns(_cnbApiOptions);

        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_cnbApiOptions.BaseUrl)
        };

        _mockHttpClientFactory
            .Setup(x => x.CreateClient(_cnbApiOptions.HttpClientName))
            .Returns(_httpClient);

        var mockLogger = new Mock<ILogger<CnbExchangeRateProvider>>();

        _provider = new CnbExchangeRateProvider(mockOptions.Object, _mockHttpClientFactory.Object, mockLogger.Object);
    }

    [Fact]
    public async Task GetLatestAsync_WithValidResponse_ReturnsCorrectExchangeRates()
    {
        // Arrange
        var cnbResponseData = new CnbExchangeRates
        {
            Rates =
            [
                new CnbExchangeRate
                {
                    Amount = 1,
                    Country = "USA",
                    Currency = "dollar",
                    CurrencyCode = "USD",
                    Order = 840,
                    Rate = 25.5m,
                    ValidFor = DateOnly.FromDateTime(DateTime.Today)
                },
                new CnbExchangeRate
                {
                    Amount = 1,
                    Country = "EMU",
                    Currency = "euro",
                    CurrencyCode = "EUR",
                    Order = 978,
                    Rate = 24.8m,
                    ValidFor = DateOnly.FromDateTime(DateTime.Today)
                },
                new CnbExchangeRate
                {
                    Amount = 100,
                    Country = "Japan",
                    Currency = "yen",
                    CurrencyCode = "JPY",
                    Order = 392,
                    Rate = 16.85m,
                    ValidFor = DateOnly.FromDateTime(DateTime.Today)
                }
            ]
        };

        var jsonResponse = JsonSerializer.Serialize(cnbResponseData);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _provider.GetLatestAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);

        var usdRate = result.First(r => r.SourceCurrency.Code == "USD");
        Assert.Equal(new Currency("USD"), usdRate.SourceCurrency);
        Assert.Equal(new Currency("CZK"), usdRate.TargetCurrency);
        Assert.Equal(25.5m, usdRate.Value); // 25.5 / 1
        Assert.Equal(DateOnly.FromDateTime(DateTime.Today), usdRate.ValidFor);

        var eurRate = result.First(r => r.SourceCurrency.Code == "EUR");
        Assert.Equal(24.8m, eurRate.Value); // 24.8 / 1

        var jpyRate = result.First(r => r.SourceCurrency.Code == "JPY");
        Assert.Equal(0.1685m, jpyRate.Value); // 16.85 / 100
    }

    [Fact]
    public async Task GetLatestAsync_WithEmptyResponse_ReturnsEmptyList()
    {
        // Arrange
        var cnbResponseData = new CnbExchangeRates
        {
            Rates = []
        };

        var jsonResponse = JsonSerializer.Serialize(cnbResponseData);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _provider.GetLatestAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetLatestAsync_ConstructsCorrectUrl()
    {
        // Arrange
        var cnbResponseData = new CnbExchangeRates { Rates = [] };
        var jsonResponse = JsonSerializer.Serialize(cnbResponseData);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"))
        };

        HttpRequestMessage? capturedRequest = null;
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(httpResponse);

        // Act
        await _provider.GetLatestAsync();

        // Assert
        Assert.NotNull(capturedRequest);
        Assert.Contains("lang=EN", capturedRequest.RequestUri?.Query);
        Assert.Contains(_cnbApiOptions.DailyExchangeRatesEndpoint, capturedRequest.RequestUri?.AbsolutePath);
    }

    [Fact]
    public async Task GetLatestAsync_WithCancellationToken_PassesTokenToHttpClient()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        
        var cnbResponseData = new CnbExchangeRates { Rates = [] };
        var jsonResponse = JsonSerializer.Serialize(cnbResponseData);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"))
        };

        var capturedTokenIsCancellationRequested = false;
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((_, token) => 
            {
                capturedTokenIsCancellationRequested = token.IsCancellationRequested;
            })
            .ReturnsAsync(httpResponse);

        // Act
        await _provider.GetLatestAsync(cancellationToken);

        // Assert
        Assert.False(capturedTokenIsCancellationRequested); // Token should not be cancelled
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task GetLatestAsync_WithHttpException_PropagatesException()
    {
        // Arrange
        var expectedException = new HttpRequestException("Network error");
        
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(expectedException);

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<HttpRequestException>(
            () => _provider.GetLatestAsync());
        
        Assert.Same(expectedException, actualException);
    }

    [Fact]
    public async Task GetLatestAsync_WithNonSuccessStatusCode_ThrowsHttpRequestException()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent("Internal server error")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            () => _provider.GetLatestAsync());
    }

    [Fact]
    public async Task GetLatestAsync_WithInvalidJsonResponse_ThrowsJsonException()
    {
        // Arrange
        const string invalidJsonResponse = "{ invalid json }";
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(invalidJsonResponse, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(
            () => _provider.GetLatestAsync());
    }

    [Fact]
    public async Task GetLatestAsync_UsesCorrectHttpClientName()
    {
        // Arrange
        var cnbResponseData = new CnbExchangeRates { Rates = [] };
        var jsonResponse = JsonSerializer.Serialize(cnbResponseData);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        await _provider.GetLatestAsync();

        // Assert
        _mockHttpClientFactory.Verify(
            x => x.CreateClient(_cnbApiOptions.HttpClientName), 
            Times.Once);
    }

    [Fact]
    public async Task GetLatestAsync_WithDifferentAmounts_CalculatesCorrectRates()
    {
        // Arrange
        var cnbResponseData = new CnbExchangeRates
        {
            Rates =
            [
                new CnbExchangeRate
                {
                    Amount = 10,
                    CurrencyCode = "SEK",
                    Rate = 23.45m,
                    Country = "Sweden",
                    Currency = "krona",
                    Order = 752,
                    ValidFor = DateOnly.FromDateTime(DateTime.Today)
                },
                new CnbExchangeRate
                {
                    Amount = 1000,
                    CurrencyCode = "KRW",
                    Rate = 18.92m,
                    Country = "South Korea",
                    Currency = "won",
                    Order = 410,
                    ValidFor = DateOnly.FromDateTime(DateTime.Today)
                }
            ]
        };

        var jsonResponse = JsonSerializer.Serialize(cnbResponseData);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _provider.GetLatestAsync();

        // Assert
        var sekRate = result.First(r => r.SourceCurrency.Code == "SEK");
        Assert.Equal(2.345m, sekRate.Value); // 23.45 / 10

        var krwRate = result.First(r => r.SourceCurrency.Code == "KRW");
        Assert.Equal(0.01892m, krwRate.Value); // 18.92 / 1000
    }

    [Fact]
    public async Task GetLatestAsync_AllExchangeRates_HaveCzkAsTargetCurrency()
    {
        // Arrange
        var cnbResponseData = new CnbExchangeRates
        {
            Rates =
            [
                new CnbExchangeRate
                {
                    Amount = 1,
                    CurrencyCode = "USD",
                    Rate = 25.5m,
                    Country = "USA",
                    Currency = "dollar",
                    Order = 840,
                    ValidFor = DateOnly.FromDateTime(DateTime.Today)
                },
                new CnbExchangeRate
                {
                    Amount = 1,
                    CurrencyCode = "EUR",
                    Rate = 24.8m,
                    Country = "EMU",
                    Currency = "euro",
                    Order = 978,
                    ValidFor = DateOnly.FromDateTime(DateTime.Today)
                }
            ]
        };

        var jsonResponse = JsonSerializer.Serialize(cnbResponseData);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _provider.GetLatestAsync();

        // Assert
        Assert.All(result, rate => Assert.Equal(new Currency("CZK"), rate.TargetCurrency));
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
