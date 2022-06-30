using System.Threading.Tasks;
using Common.Exceptions;
using ExchangeRate.Provider.Cnb.HttpClient;
using ExchangeRate.Service.UnitTests.Enums;
using ExchangeRate.Service.UnitTests.Mock;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRate.Service.UnitTests.Tests;

[TestClass]
public class CnbHttpClientTests
{
    [TestMethod]
    public async Task CnbHttpClient_GetExchangeRate_Czech_Valid()
    {
        // Arrange
        var httpClientFactory = MockHttpClient.GetMockHttpClientFactory(MockData.ExchangeRateEnglishSample(CnbSourceLanguage.Czech));
        var clientConfiguration = Options.Create(MockData.CnbProviderConfigurationSample(CnbSourceLanguage.Czech));
        var client = new CnbHttpClient(httpClientFactory.Object, clientConfiguration);

        // Act
        var response = await client.GetExchangeRate();

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(3, response.Count);

        var actualEu = response[0];
        Assert.AreEqual("EMU", actualEu.Country);
        Assert.AreEqual("euro", actualEu.Currency);
        Assert.AreEqual(1, actualEu.Amount);
        Assert.AreEqual("EUR", actualEu.Code);
        Assert.AreEqual(24.730m, actualEu.Rate);

        var actualSingaporeDollar = response[1];
        Assert.AreEqual("Singapur", actualSingaporeDollar.Country);
        Assert.AreEqual("dollar", actualSingaporeDollar.Currency);
        Assert.AreEqual(1, actualSingaporeDollar.Amount);
        Assert.AreEqual("SGD", actualSingaporeDollar.Code);
        Assert.AreEqual(16.916m, actualSingaporeDollar.Rate);

        var actualUsDollar = response[2];
        Assert.AreEqual("USA", actualUsDollar.Country);
        Assert.AreEqual("dollar", actualUsDollar.Currency);
        Assert.AreEqual(1, actualUsDollar.Amount);
        Assert.AreEqual("USD", actualUsDollar.Code);
        Assert.AreEqual(23.501m, actualUsDollar.Rate);
    }

    [TestMethod]
    public async Task CnbHttpClient_GetExchangeRate_English_Valid()
    {
        // Arrange
        var httpClientFactory = MockHttpClient.GetMockHttpClientFactory(MockData.ExchangeRateEnglishSample(CnbSourceLanguage.English));
        var clientConfiguration = Options.Create(MockData.CnbProviderConfigurationSample(CnbSourceLanguage.English));
        var client = new CnbHttpClient(httpClientFactory.Object, clientConfiguration);

        // Act
        var response = await client.GetExchangeRate();

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(3, response.Count);

        var actualEu = response[0];
        Assert.AreEqual("EMU", actualEu.Country);
        Assert.AreEqual("euro", actualEu.Currency);
        Assert.AreEqual(1, actualEu.Amount);
        Assert.AreEqual("EUR", actualEu.Code);
        Assert.AreEqual(24.730m, actualEu.Rate);

        var actualSingaporeDollar = response[1];
        Assert.AreEqual("Singapore", actualSingaporeDollar.Country);
        Assert.AreEqual("dollar", actualSingaporeDollar.Currency);
        Assert.AreEqual(1, actualSingaporeDollar.Amount);
        Assert.AreEqual("SGD", actualSingaporeDollar.Code);
        Assert.AreEqual(16.916m, actualSingaporeDollar.Rate);

        var actualUsDollar = response[2];
        Assert.AreEqual("USA", actualUsDollar.Country);
        Assert.AreEqual("dollar", actualUsDollar.Currency);
        Assert.AreEqual(1, actualUsDollar.Amount);
        Assert.AreEqual("USD", actualUsDollar.Code);
        Assert.AreEqual(23.501m, actualUsDollar.Rate);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidContentException))]
    [DataRow(MockData.ExchangeRateEnglishSampleMissingColumnHeader)]
    [DataRow(MockData.ExchangeRateEnglishSampleMissingDateHeader)]
    public async Task CnbHttpClient_GetExchangeRate_English_Invalid_Structure(string structure)
    {
        // Arrange
        var httpClientFactory = MockHttpClient.GetMockHttpClientFactory(structure);
        var clientConfiguration = Options.Create(MockData.CnbProviderConfigurationSample(CnbSourceLanguage.English));
        var client = new CnbHttpClient(httpClientFactory.Object, clientConfiguration);

        // Act
        await client.GetExchangeRate();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidContentException))]
    [DataRow("abc")]
    [DataRow(MockData.ExchangeRateCzechSampleMissingColumnHeader)]
    [DataRow(MockData.ExchangeRateCzechSampleMissingDateHeader)]
    [DataRow("")]
    public async Task CnbHttpClient_GetExchangeRate_Czech_Invalid_Structure(string structure)
    {
        // Arrange
        var httpClientFactory = MockHttpClient.GetMockHttpClientFactory(structure);
        var clientConfiguration = Options.Create(MockData.CnbProviderConfigurationSample(CnbSourceLanguage.English));
        var client = new CnbHttpClient(httpClientFactory.Object, clientConfiguration);

        // Act
        await client.GetExchangeRate();
    }
}