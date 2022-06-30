using System.Linq;
using System.Threading.Tasks;
using Common.Exceptions;
using ExchangeRate.Provider.Cnb.Interfaces;
using ExchangeRate.Provider.Cnb.Models.Configuration;
using ExchangeRate.Service.Enums;
using ExchangeRate.Service.UnitTests.Enums;
using ExchangeRate.Service.UnitTests.Mock;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ExchangeRate.Service.UnitTests.Tests;

[TestClass]
public class ExchangeRateServiceTests
{
    [TestMethod]
    public async Task ExchangeRateService_Result_Ok()
    {
        // Arrange
        var httpClient = new Mock<ICnbHttpClient>();
        var mockCache = GetMockMemoryCache();
        var clientConfiguration = Options.Create(MockData.CnbProviderConfigurationSample(CnbSourceLanguage.English));

        httpClient.Setup(s => s.GetExchangeRate()).ReturnsAsync(MockData.ExchangeRatesEnglishSampleList);

        var exchangeRateService = new ExchangeRateService(httpClient.Object, mockCache!, clientConfiguration);

        // Act
        var result = (await exchangeRateService.GetExchangeRates(ProviderSource.Cnb)).ToArray();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Length);

        Assert.AreEqual("USD", result[0].SourceCurrency.Code);
        Assert.AreEqual("CZK", result[0].TargetCurrency.Code);
        Assert.AreEqual(23.501m, result[0].Value);

        Assert.AreEqual("EUR", result[1].SourceCurrency.Code);
        Assert.AreEqual("CZK", result[1].TargetCurrency.Code);
        Assert.AreEqual(24.730m, result[1].Value);
    }

    [TestMethod]
    [ExpectedException(typeof(ConfigurationException), "Configuration was not se properly")]
    public async Task ExchangeRateService_Wrong_Configuration()
    {
        // Arrange
        var httpClient = new Mock<ICnbHttpClient>();
        var mockCache = new Mock<IMemoryCache>();
        var clientConfiguration = Options.Create(new CnbProviderConfiguration());

        httpClient.Setup(s => s.GetExchangeRate()).ReturnsAsync(MockData.ExchangeRatesEnglishSampleList);

        var exchangeRateService = new ExchangeRateService(httpClient.Object, mockCache.Object, clientConfiguration);

        // Act
        await exchangeRateService.GetExchangeRates(ProviderSource.Cnb);
    }

    [TestMethod]
    [ExpectedException(typeof(ConfigurationException), "Configuration was not se properly")]
    public async Task ExchangeRateService_Invalid_Data()
    {
        // Arrange
        var httpClient = new Mock<ICnbHttpClient>();
        var mockCache = new Mock<IMemoryCache>();
        var clientConfiguration = Options.Create(new CnbProviderConfiguration());

        httpClient.Setup(s => s.GetExchangeRate()).ReturnsAsync(MockData.ExchangeRatesEnglishSampleList);

        var exchangeRateService = new ExchangeRateService(httpClient.Object, mockCache.Object, clientConfiguration);

        // Act
        await exchangeRateService.GetExchangeRates(ProviderSource.Cnb);
    }

    private static IMemoryCache? GetMockMemoryCache()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider.GetService<IMemoryCache>();
    }
}