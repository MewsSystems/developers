using System.Net;
using ExchangeRateUpdater;
using ExchangeRateUpdaterAPI.Services.ExchangeRateFormatterService;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ExchangeRateUpdaterAPITests;

public class GetExchangeRateProviderTests
{
    [Test]
    public async Task GetExchangeRates_WhenExchangeRateDataSourceNotConfigured_ThrowsArgumentException()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        var httpClient = new HttpClient();
        var exchangeRateFormatter = new Mock<IExchangeRateFormatter>().Object;
        var exchangeRateProvider = new ExchangeRateProvider(configuration, httpClient, exchangeRateFormatter);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => exchangeRateProvider.GetExchangeRates(new List<Currency>()));
    }


    [Test]
    public async Task GetExchangeRates_WhenExchangeRateDataSourceFileDoesNotExist_ThrowsFileNotFoundException()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
            {"ExchangeRateDateSource", "invalid-path.csv"}
            }).Build();
        var httpClient = new HttpClient();
        var exchangeRateFormatter = new Mock<IExchangeRateFormatter>().Object;
        var exchangeRateProvider = new ExchangeRateProvider(configuration, httpClient, exchangeRateFormatter);

        // Act & Assert
        Assert.ThrowsAsync<FileNotFoundException>(() => exchangeRateProvider.GetExchangeRates(new List<Currency>()));
    }

    [Test]
    public async Task GetExchangeRates_WhenExchangeRateDataSourceContentIsEmpty_ThrowsException()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
            {"ExchangeRateDateSource", "valid-path.csv"}
            }).Build();
        var httpClient = new HttpClient(new EmptyResponseHandler());
        var exchangeRateFormatter = new Mock<IExchangeRateFormatter>().Object;
        var exchangeRateProvider = new ExchangeRateProvider(configuration, httpClient, exchangeRateFormatter);

        // Act & Assert
        Assert.ThrowsAsync<FileNotFoundException>(() => exchangeRateProvider.GetExchangeRates(new List<Currency>()));
    }

    public class EmptyResponseHandler : HttpMessageHandler
    {
        // For GetExchangeRates_WhenExchangeRateDataSourceContentIsValid_ReturnsExchangeRates
        public HttpResponseMessage ResponseMessage { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(ResponseMessage ?? new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty)
            });
        }
    }
}