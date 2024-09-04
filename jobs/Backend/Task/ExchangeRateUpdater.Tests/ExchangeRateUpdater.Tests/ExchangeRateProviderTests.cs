using System.Net;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    private const string _url = "https://example.com";
    
    [Fact(DisplayName = "GetExchangeRates with 0 results from API")]
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
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, _url);
    
        // Act
        var result = await exchangeRateProvider.GetExchangeRates(MockData.Currencies.DefaultCurrencies);
    
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
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, _url);
    
        // Act
        var result = (await exchangeRateProvider.GetExchangeRates(MockData.Currencies.DefaultCurrencies)).ToArray();
    
        // Assert
        Assert.Contains(new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.070m), result);
        Assert.Contains(new ExchangeRate(new Currency("JPY"), new Currency("CZK"), 0.15546m), result);
        Assert.Contains(new ExchangeRate(new Currency("THB"), new Currency("CZK"), 0.66281m), result);
        Assert.Contains(new ExchangeRate(new Currency("TRY"), new Currency("CZK"), 0.6684m), result);
        Assert.Contains(new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.711m), result);
    }
    
    [Fact(DisplayName = "GetExchangeRates with normal results from API, with non-existing currency")]
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
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, _url);
    
        // Act
        var result = (await exchangeRateProvider.GetExchangeRates(MockData.Currencies.NonExistingCurrency)).ToArray();
    
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
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, _url);

        // Act
        var result = (await exchangeRateProvider.GetExchangeRates(MockData.Currencies.EmptyCurrencies)).ToArray();

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
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, _url);

        // Act
        var result = (await exchangeRateProvider.GetExchangeRates(MockData.Currencies.DefaultCurrencies)).ToArray();

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
        
        var exchangeRateProvider = new ExchangeRateProvider(httpClient, _url);

        // Act
        var result = (await exchangeRateProvider.GetExchangeRates(MockData.Currencies.DefaultCurrencies)).ToArray();

        // Assert
        Assert.Empty(result);
    }
}