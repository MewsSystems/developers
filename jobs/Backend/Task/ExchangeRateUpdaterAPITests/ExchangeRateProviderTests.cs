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
}