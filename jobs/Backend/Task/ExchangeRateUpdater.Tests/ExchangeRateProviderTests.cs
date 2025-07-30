using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class ExchangeRateProviderTests
{
    private Mock<IMemoryCache> _mockCache;
    private IConfiguration _configuration;
    private ExchangeRateSettingsResolver _settingsResolver;
    private ExchangeRateProvider _provider;

    [SetUp]
    public void Setup()
    {
        _mockCache = new Mock<IMemoryCache>();

        _configuration = new ConfigurationBuilder()
            .SetBasePath(TestContext.CurrentContext.TestDirectory)
            .AddJsonFile("appsettings.test.json", optional: false)
            .Build();

        _settingsResolver = new ExchangeRateSettingsResolver(_configuration);
        _mockCache.Setup(m => m.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns(false);
        _mockCache.Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(Mock.Of<ICacheEntry>());

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(TestData.CnbExchangeRateXml),
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        _provider = new ExchangeRateProvider(_mockCache.Object, httpClient, _settingsResolver, _configuration);
    }

    [Test]
    public async Task GetExchangeRates_ValidCurrencies_ReturnsRates()
    {
        // Arrange
        var currencies = TestData.ValidCurrenciesForTest;
        var baseCurrency = new Currency(TestData.BaseCurrency);
        var expectedRates = new List<ExchangeRate>
        {
            new ExchangeRate(baseCurrency, new Currency("EUR"), 24.610m),
            new ExchangeRate(baseCurrency, new Currency("USD"), 21.331m)
        };
        
        // Act
        var result = (await _provider.GetExchangeRates(currencies, baseCurrency)).ToList();

        // Assert
        Assert.That(result, Is.EquivalentTo(expectedRates)
            .Using<ExchangeRate>((x, y) => x.SourceCurrency.Code == y.SourceCurrency.Code &&
                                                                     x.TargetCurrency.Code == y.TargetCurrency.Code &&
                                                                     x.Value == y.Value));
    }


    [TestCase("TEST")]  
    [TestCase("")]  
    public void GetExchangeRates_InvalidBaseCurrency_ThrowsArgumentException(string currencyCode)
    {
        // Arrange
        var currencies = TestData.ValidCurrenciesForTest;
        var baseCurrency = new Currency(currencyCode);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _provider.GetExchangeRates(currencies, baseCurrency));
    }
    [Test]
    public void GetExchangeRates_NotImplementedBaseCurrency_ThrowsApiException()
    {
        // Arrange
        var currencies = TestData.ValidCurrenciesForTest;
        var baseCurrency = new Currency("USD");

        // Act & Assert
        Assert.ThrowsAsync<ExchangeRateApiException>(() => _provider.GetExchangeRates(currencies, baseCurrency));
    }

    [Test]
    public void GetExchangeRates_InvalidTargetCurrency_ThrowsArgumentException()
    {
        // Arrange
        var currencies = TestData.InvalidCurrenciesForTest;
        var baseCurrency = new Currency(TestData.BaseCurrency);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _provider.GetExchangeRates(currencies, baseCurrency));
    }
    [Test]
    public void GetExchangeRates_EmptyTargetCurrencies_ThrowsArgumentException()
    {
        // Arrange
        var baseCurrency = new Currency(TestData.BaseCurrency);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _provider.GetExchangeRates([], baseCurrency));
    }
}


