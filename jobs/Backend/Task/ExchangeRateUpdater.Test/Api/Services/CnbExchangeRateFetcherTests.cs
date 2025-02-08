using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Api.Services;
using ExchangeRateUpdater.Contract;
using ExchangeRateUpdater.Contract.ExchangeRate;
using ExchangeRateUpdater.Test.Support;
using Microsoft.Extensions.Logging;
using NFluent;
using NSubstitute;

namespace ExchangeRateUpdater.Test.Api.Services;

public sealed class CnbExchangeRateFetcherTests
{
    private ILogger<CnbExchangeRateFetcher> _logger;
    private ResourcesConfiguration _resourcesConfig;

    [SetUp]
    public void SetUp()
    {
        _logger = Substitute.For<ILogger<CnbExchangeRateFetcher>>();
        _resourcesConfig = new ResourcesConfiguration { CnbApiUrl = "https://api.example.com/rates" };
    }

    [Test]
    public async Task FetchExchangeRatesAsync_ShouldReturnSuccess_WhenValidDataIsFetched()
    {
        // Arrange
        var expectedRates = new List<CnbExchangeRate>
        {
            new(Order: 94, Country: "USA", Currency: "dollar", CurrencyCode: Currency.Usd, Amount: 1, Rate: 23.048m),
            new(Order: 94, Country: "EMU", Currency: "euro", CurrencyCode: Currency.Eur, Amount: 1, Rate: 25.75m),
            new(Order: 94, Country: "Filipíny", Currency: "peso", CurrencyCode: Currency.Php, Amount: 100, Rate: 43.71m)
        };

        var jsonResponse = JsonSerializer.Serialize(new CnbExchangeRatesResponse(expectedRates));
        var httpClient = new HttpClient(new HttpMessageHandlerStub(jsonResponse));
        var fetcher = new CnbExchangeRateFetcher(_logger, httpClient, _resourcesConfig);

        // Act
        var result = await fetcher.FetchExchangeRatesAsync(CancellationToken.None);

        // Assert
        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Success.Get()).ContainsExactly(expectedRates);
    }

    [Test]
    public async Task FetchExchangeRatesAsync_ShouldReturnError_WhenResponseIsInvalidJson()
    {
        // Arrange
        const string invalidJson = "INVALID_JSON";

        var httpClient = new HttpClient(new HttpMessageHandlerStub(invalidJson));
        var fetcher = new CnbExchangeRateFetcher(_logger, httpClient, _resourcesConfig);

        // Act
        var result = await fetcher.FetchExchangeRatesAsync(CancellationToken.None);

        // Assert
        Check.That(result.IsError).IsTrue();
        Check.That(result.Error.Get()).IsEqualTo(CnbExchangeRatesFetchError.DataFormat);
    }

    [Test]
    public async Task FetchExchangeRatesAsync_ShouldReturnNoDataError_WhenResponseIsNull()
    {
        // Arrange
        const string nullResponse = "null";
        var httpClient = new HttpClient(new HttpMessageHandlerStub(nullResponse));
        var fetcher = new CnbExchangeRateFetcher(_logger, httpClient, _resourcesConfig);

        // Act
        var result = await fetcher.FetchExchangeRatesAsync(CancellationToken.None);

        // Assert
        Check.That(result.IsError).IsTrue();
        Check.That(result.Error.Get()).IsEqualTo(CnbExchangeRatesFetchError.NoData);
    }

    [Test]
    public async Task FetchExchangeRatesAsync_ShouldReturnTimeoutError_WhenRequestIsCancelled()
    {
        // Arrange
        var exception = new TaskCanceledException();
        var httpClient = new HttpClient(new HttpMessageHandlerStub(exception));
        var fetcher = new CnbExchangeRateFetcher(_logger, httpClient, _resourcesConfig);

        // Act
        var result = await fetcher.FetchExchangeRatesAsync(CancellationToken.None);

        // Assert
        Check.That(result.IsError).IsTrue();
        Check.That(result.Error.Get()).IsEqualTo(CnbExchangeRatesFetchError.Timeout);
    }

    [Test]
    public async Task FetchExchangeRatesAsync_ShouldReturnServerError_WhenServerReturnsError()
    {
        // Arrange
        var webResponse = Substitute.For<HttpWebResponse>();

        webResponse.StatusCode.Returns(HttpStatusCode.BadGateway);
        
        var exception = new WebException("Server error", null, WebExceptionStatus.ProtocolError, webResponse);
        var httpClient = new HttpClient(new HttpMessageHandlerStub(exception));
        var fetcher = new CnbExchangeRateFetcher(_logger, httpClient, _resourcesConfig);

        // Act
        var result = await fetcher.FetchExchangeRatesAsync(CancellationToken.None);

        // Assert
        Check.That(result.IsError).IsTrue();
        Check.That(result.Error.Get()).IsEqualTo(CnbExchangeRatesFetchError.ServerException);
    }

    [Test]
    public async Task FetchExchangeRatesAsync_ShouldReturnNetworkIssuesError_WhenConnectionFails()
    {
        // Arrange
        var exception = new WebException("Connection failed", WebExceptionStatus.ConnectFailure);
        var httpClient = new HttpClient(new HttpMessageHandlerStub(exception));
        var fetcher = new CnbExchangeRateFetcher(_logger, httpClient, _resourcesConfig);
        
        // Act
        var result = await fetcher.FetchExchangeRatesAsync(CancellationToken.None);

        // Assert
        Check.That(result.IsError).IsTrue();
        Check.That(result.Error.Get()).IsEqualTo(CnbExchangeRatesFetchError.NetworkIssues);
    }

    [Test]
    public async Task FetchExchangeRatesAsync_ShouldReturnUnknownError_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var exception = new Exception("Unexpected error");
        var httpClient = new HttpClient(new HttpMessageHandlerStub(exception));
        var fetcher = new CnbExchangeRateFetcher(_logger, httpClient, _resourcesConfig);

        // Act
        var result = await fetcher.FetchExchangeRatesAsync(CancellationToken.None);

        // Assert
        Check.That(result.IsError).IsTrue();
        Check.That(result.Error.Get()).IsEqualTo(CnbExchangeRatesFetchError.Unknown);
    }
}