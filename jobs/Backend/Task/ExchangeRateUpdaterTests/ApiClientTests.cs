using System.Net;
using ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient;
using ExchangeRateUpdater.ConfigurationOptions;
using ExchangeRateUpdater.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace ExchangeRateUpdaterTests;

public class ApiClientTests
{
    private readonly MockHttpMessageHandler _httpMessageHandlerMock;
    private readonly Mock<IOptions<CnbClientOptions>> _cnbClientOptionsMock;
    private readonly Mock<ILogger<ApiClient>> _loggerMock;
    private readonly TimeProvider _timeProvider;
    private const string BaseUri = "http://test";

    public ApiClientTests()
    {
        _timeProvider = TimeProvider.System;
        _loggerMock = new Mock<ILogger<ApiClient>>();
        _httpMessageHandlerMock = new MockHttpMessageHandler();

        _cnbClientOptionsMock = new Mock<IOptions<CnbClientOptions>>();
        _cnbClientOptionsMock.Setup(x => x.Value).Returns(new CnbClientOptions
            { BaseUri = BaseUri, DailyExchangeRatesPath = "/path" });
    }

    [Fact]
    public async Task GetDailyExchangeRatesAsync_WhenApiReturnsSuccess_ShouldReturnValidExchangeRates()
    {
        // Arrange
        _httpMessageHandlerMock.When($"{BaseUri}/*")
            .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(ValidApiResponseBody)
            });

        var httpClient = _httpMessageHandlerMock.ToHttpClient();
        httpClient.BaseAddress = new Uri(BaseUri);

        var apiClient = new ApiClient(httpClient, _cnbClientOptionsMock.Object, _loggerMock.Object, _timeProvider);

        // Act
        var result = await apiClient.GetDailyExchangeRatesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Rates);
        Assert.Equal(14, result.Rates.Count);
    }

    [Fact]
    public async Task GetDailyExchangeRatesAsync_WhenApiReturnsInvalidResponse_ShouldThrowCnbApiClientException()
    {
        // Arrange
        _httpMessageHandlerMock.When($"{BaseUri}/*")
            .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(InvalidApiResponseBody)
            });

        var httpClient = _httpMessageHandlerMock.ToHttpClient();
        httpClient.BaseAddress = new Uri(BaseUri);

        var apiClient = new ApiClient(httpClient, _cnbClientOptionsMock.Object, _loggerMock.Object, _timeProvider);

        // Act && Assert
        await Assert.ThrowsAsync<CnbApiClientException>(async () => await apiClient.GetDailyExchangeRatesAsync());
    }

    [Fact]
    public async Task GetDailyExchangeRatesAsync_WhenApiReturnsBadResponse_ShouldThrowCnbApiClientException()
    {
        // Arrange
        _httpMessageHandlerMock.When($"{BaseUri}/*")
            .Respond(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError));

        var httpClient = _httpMessageHandlerMock.ToHttpClient();
        httpClient.BaseAddress = new Uri(BaseUri);

        var apiClient = new ApiClient(httpClient, _cnbClientOptionsMock.Object, _loggerMock.Object, _timeProvider);

        // Act && Assert
        await Assert.ThrowsAsync<CnbApiClientException>(async () => await apiClient.GetDailyExchangeRatesAsync());
    }

    private const string ValidApiResponseBody =
        """
          {
          "rates": [
             {
               "currencyCode": "AUD",
               "rate": 15.061,
               "amount": 1
             },
             {
               "currencyCode": "EUR",
               "rate": 24.655,
               "amount": 1
             },
             {
               "currencyCode": "HKD",
               "rate": 2.87,
               "amount": 1
             },
             {
               "currencyCode": "HUF",
               "rate": 6.508,
               "amount": 1
             },
             {
               "currencyCode": "ISK",
               "rate": 16.426,
               "amount": 1
             },
             {
               "currencyCode": "XDR",
               "rate": 29.981,
               "amount": 1
             },
             {
               "currencyCode": "INR",
               "rate": 27.031,
               "amount": 100
             },
             {
               "currencyCode": "IDR",
               "rate": 1.443,
               "amount": 1
             },
             {
               "currencyCode": "JPY",
               "rate": 15.436,
               "amount": 1
             },
             {
               "currencyCode": "MYR",
               "rate": 4.83,
               "amount": 1
             },
             {
               "currencyCode": "MXN",
               "rate": 1.321,
               "amount": 1
             },
             {
               "currencyCode": "NZD",
               "rate": 14.021,
               "amount": 1
             },
             {
               "currencyCode": "GBP",
               "rate": 28.613,
               "amount": 1
             },
             {
               "currencyCode": "USD",
               "rate": 22.44,
               "amount": 1
             }
            ]
          }
        """;

    private const string InvalidApiResponseBody =
        """
          {
          "rates": [
             {
               "currencyCode": "AUD",
               "rates": 15.061,
               "amount": 1
             },
             {
               "currencyCode": "EUR",
               "rate": 24.655,
               "amount": 1
             }
            ]
          }
        """;
}