using Xunit;
using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;

public class ExchangeRateProviderEdgeCaseTests
{
    private readonly ExchangeRateProvider _provider;

    public ExchangeRateProviderEdgeCaseTests()
    {
        var loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["ExchangeRateApi:BaseUrl"]).Returns("https://api.cnb.cz/cnbapi/exrates/daily");

        var httpClient = new HttpClient(SetupMockHttpMessageHandler("{\"rates\":[]}"))
        {
            BaseAddress = new Uri("https://api.cnb.cz/cnbapi/exrates/daily")
        };

        _provider = new ExchangeRateProvider(loggerMock.Object, configurationMock.Object, httpClient);
    }

    [Fact]
    public async Task GetExchangeRates_WithEmptyCurrencyList_ReturnsEmptyResult()
    {
        var currencies = new List<Currency>();
        var result = await _provider.GetExchangeRates(currencies);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRates_WithInvalidCurrencyCode_ReturnsNoMatch()
    {
        var currencies = new List<Currency> { new Currency("InvalidCurrencyCode") };
        var httpClient = new HttpClient(SetupMockHttpMessageHandler(
            "{\"rates\":[" +
            "{\"validFor\":\"2023-12-15\", \"currencyCode\":\"AUD\", \"rate\":15.004}," +
            "{\"validFor\":\"2023-12-15\", \"currencyCode\":\"EUR\", \"rate\":24.480}," +
            "{\"validFor\":\"2023-12-15\", \"currencyCode\":\"USD\", \"rate\":22.364}" +
            "]}"))
        {
            BaseAddress = new Uri("https://api.cnb.cz/cnbapi/exrates/daily")
        };

        var newProvider = new ExchangeRateProvider(new Mock<ILogger<ExchangeRateProvider>>().Object, new Mock<IConfiguration>().Object, httpClient);
        var result = await newProvider.GetExchangeRates(currencies);
        Assert.Empty(result); 
    }

    private HttpMessageHandler SetupMockHttpMessageHandler(string content)
    {
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            });

        return httpMessageHandlerMock.Object;
    }
}
