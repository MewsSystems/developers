using ExchangeRate.Provider.Cnb;
using ExchangeRate.Provider.Cnb.Factory;
using ExchangeRate.Provider.Cnb.HttpClient;
using ExchangeRate.Service.UnitTests.Enums;
using ExchangeRate.Service.UnitTests.Mock;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRate.Service.UnitTests.Tests;

[TestClass]
public class CnbProviderTests
{
    [TestMethod]
    public void ConcreteCnbProvider_Create_Ok()
    {
        // Arrange
        var httpClientFactory = MockHttpClient.GetMockHttpClientFactory(MockData.ExchangeRateEnglishSample(CnbSourceLanguage.Czech));
        var clientConfiguration = Options.Create(MockData.CnbProviderConfigurationSample(CnbSourceLanguage.Czech));
        var client = new CnbHttpClient(httpClientFactory.Object, clientConfiguration);

        // Act
        var provider = new CnbProviderCreator(client, clientConfiguration).CreateProvider();

        // Assert
        Assert.IsNotNull(provider);
        Assert.IsInstanceOfType(provider, typeof(ConcreteCnbProvider));
    }
}