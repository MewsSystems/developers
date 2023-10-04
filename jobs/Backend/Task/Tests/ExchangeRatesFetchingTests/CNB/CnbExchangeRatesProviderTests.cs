using ExchangeRatesFetching.CNB;
using ExchangeRatesUpdater.Common;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;

namespace ExchangeRatesFetchingTests.CNB;

public class CnbExchangeRatesProviderTests
{
    private const string correctCurrencyCode = "AUD";

    private readonly IOptions<AppConfiguration> configMock;
    private readonly ILogger<CnbExchangeRatesProvider> loggerMock;
    private readonly IHttpClientFactory httpClientFactoryMock;

    public CnbExchangeRatesProviderTests()
    {
        configMock = CreateConfigurationMock();
        loggerMock = new Mock<ILogger<CnbExchangeRatesProvider>>().Object;
        httpClientFactoryMock = CreateHttpClientFactoryMock();
    }

    private static IOptions<AppConfiguration> CreateConfigurationMock()
    {
        Mock<IOptions<AppConfiguration>> mockConfig = new();
        AppConfiguration appConfig = new() {
            ProviderAPIs = new Dictionary<string, string>
            {
                { Constants.CnbName, "https://mocked.address" }
            }
        };
        mockConfig.Setup(m => m.Value).Returns(appConfig);

        return mockConfig.Object;
    }

    private static IHttpClientFactory CreateHttpClientFactoryMock()
    {
        Mock<IHttpClientFactory> mockClientFactory = new();
        Mock<HttpMessageHandler> mockHttpClientHandler = new();
        HttpClient httpClient = new(mockHttpClientHandler.Object);
        mockClientFactory.Setup(m => m.CreateClient(It.IsAny<string>())).Returns(httpClient);

        mockHttpClientHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
                    ""rates"": [
                    {
                        ""validFor"": ""2019-05-17"",
                        ""order"": 94,
                        ""country"": ""Australia"",
                        ""currency"": ""dollar"",
                        ""amount"": 1,
                        ""currencyCode"": ""AUD"",
                        ""rate"": 15.858
                    }]
                }"),
            });

        return mockClientFactory.Object;
    }

    [Fact]
    public async Task GetRatesForCurrenciesAsync_WithOneMatchingCurrency_ReturnsSingleCorrectResult()
    {
        // Arrange
        CnbExchangeRatesProvider provider = new(configMock, loggerMock, httpClientFactoryMock);

        // Act
        IEnumerable<ExchangeRate> result = await provider.GetRatesForCurrenciesAsync(new List<string> { "CZK", correctCurrencyCode });

        // Assert
        Assert.Single(result);
        Assert.True(result.Single().SourceCurrency.Code == correctCurrencyCode);
    }

    [Fact]
    public async Task GetRatesForCurrenciesAsync_WithNoMatchingCurrencies_ReturnsNoResults()
    {
        // Arrange
        CnbExchangeRatesProvider provider = new(configMock, loggerMock, httpClientFactoryMock);

        // Act
        IEnumerable<ExchangeRate> result = await provider.GetRatesForCurrenciesAsync(new List<string> { "CZK" });

        // Assert
        Assert.Empty(result);
    }
}
