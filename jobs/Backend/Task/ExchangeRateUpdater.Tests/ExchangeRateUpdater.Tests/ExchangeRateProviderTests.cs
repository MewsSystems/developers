using System.Net;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    private const string _dummyUrl = "https://example.com";
    
    [Fact(DisplayName = "GetExchangeRates with the API returning 0 results")]
    public async Task GetExchangeRates_0Results()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(MockData.ResponseBodies.Empty)
            });
        
        using var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var exchangeRateProvider = new ExchangeRateProvider(httpClient, Options.Create(new ExchangeRatesConfig
        {
            Url = _dummyUrl,
            Currencies = MockData.Currencies.DefaultCurrencies
        }));
    
        // Act
        var result = await exchangeRateProvider.GetExchangeRates();
    
        // Assert
        Assert.Empty(result);
    }
    
    [Fact(DisplayName = "GetExchangeRates with normal results from API")]
    public async Task GetExchangeRates_Success()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(MockData.ResponseBodies.Default)
            });
        
        using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, Options.Create(new ExchangeRatesConfig
        {
            Url = _dummyUrl,
            Currencies = MockData.Currencies.DefaultCurrencies
        }));
    
        // Act
        var result = (await exchangeRateProvider.GetExchangeRates()).ToArray();
    
        // Assert
        Assert.Contains(new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.070m), result);
        Assert.Contains(new ExchangeRate(new Currency("JPY"), new Currency("CZK"), 0.15546m), result);
        Assert.Contains(new ExchangeRate(new Currency("THB"), new Currency("CZK"), 0.66281m), result);
        Assert.Contains(new ExchangeRate(new Currency("TRY"), new Currency("CZK"), 0.6684m), result);
        Assert.Contains(new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.711m), result);
    }
    
    [Fact(DisplayName = "GetExchangeRates with normal results from API, with a non-existing currency")]
    public async Task GetExchangeRates_NonExistingCurrency()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(MockData.ResponseBodies.Default)
            });
        
        using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, Options.Create(new ExchangeRatesConfig
        {
            Url = _dummyUrl,
            Currencies = MockData.Currencies.NonExistingCurrency
        }));
    
        // Act
        var result = (await exchangeRateProvider.GetExchangeRates()).ToArray();
    
        // Assert
        Assert.Empty(result);
    }
    
    [Fact(DisplayName = "GetExchangeRates with normal results from API, with empty currencies")]
    public async Task GetExchangeRates_EmptyCurrencies()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(MockData.ResponseBodies.Default)
            });
        
        using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, Options.Create(new ExchangeRatesConfig
        {
            Url = _dummyUrl,
            Currencies = MockData.Currencies.EmptyCurrencies
        }));
        
        // Act
        var result = (await exchangeRateProvider.GetExchangeRates()).ToArray();

        // Assert
        Assert.Empty(result);
    }
    
    [Fact(DisplayName = "GetExchangeRates with the API returning 400 Bad Request")]
    public async Task GetExchangeRates_BadRequest()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));
        
        using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, Options.Create(new ExchangeRatesConfig
        {
            Url = _dummyUrl,
            Currencies = MockData.Currencies.DefaultCurrencies
        }));

        // Act
        var result = (await exchangeRateProvider.GetExchangeRates()).ToArray();

        // Assert
        Assert.Empty(result);
    }
    
    [Fact(DisplayName = "GetExchangeRates with the API timing out")]
    public async Task GetExchangeRates_Timeout()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException());
        
        using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, Options.Create(new ExchangeRatesConfig
        {
            Url = _dummyUrl,
            Currencies = MockData.Currencies.DefaultCurrencies
        }));

        // Act
        var result = (await exchangeRateProvider.GetExchangeRates()).ToArray();

        // Assert
        Assert.Empty(result);
    }
}