using NUnit.Framework;
using Moq;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Moq.Protected;
using System;
using System.Net.Http;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Controllers;
using ExchangeRateUpdater.Parsers;


namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class ExchangeRateControllerTests
{
    private ExchangeRateController _controller;
    private IConfiguration _configuration;
    private Mock<IMemoryCache> _mockCache;
    private ExchangeRateSettingsResolver _settingsResolver;
    private ExchangeRateProvider _provider;

    [SetUp]
    public void Setup()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(TestContext.CurrentContext.TestDirectory)
            .AddJsonFile("appsettings.test.json", optional: false)
            .Build();
        _mockCache = new Mock<IMemoryCache>();


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
        _controller = new ExchangeRateController(_provider);
    }

    [Test]
    public async Task GetExchangeRates_ValidCurrencies_ReturnsOkWithRates()
    {
        // Arrange
        var currencies = "EUR,USD";
        var baseCurrency = "CZK";
        var expectedRates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("CZK"), new Currency("EUR"), 24.610m),
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 21.331m)
        };

        // Act
        var result = await _controller.GetExchangeRates(currencies, baseCurrency);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedRates = okResult.Value as IEnumerable<ExchangeRateResponse>;
        Assert.That(returnedRates, Is.Not.Null);
        Assert.That(returnedRates.Count(), Is.EqualTo(expectedRates.Count));
    }

    [Test]
    public async Task GetExchangeRates_EmptyCurrencies_ReturnsBadRequest()
    {
        // Arrange
        var currencies = "";
        var baseCurrency = "CZK";

        // Act
        var result = await _controller.GetExchangeRates(currencies, baseCurrency);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.EqualTo("Currency codes cannot be empty."));
    }

    [Test]
    public async Task GetExchangeRates_ArgumentExceptionFromProvider_ReturnsBadRequest()
    {
        // Arrange
        var currency = "TEST";
        var baseCurrency = "CZK";
        var errorMessage = $"The following currencies are not allowed: {currency}";


        // Act
        var result = await _controller.GetExchangeRates(currency, baseCurrency);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
    }

    [Test]
    public async Task GetExchangeRates_ExchangeRateApiExceptionFromProvider_ReturnsServiceUnavailable()
    {
        // Arrange
        var currencies = "EUR";
        var baseCurrency = "CZK";


        var httpClient = new HttpClient();


        var provider = new ExchangeRateProvider( _mockCache.Object, httpClient, _settingsResolver, _configuration);
        var controller = new ExchangeRateController(provider);

        // Act
        var result = await controller.GetExchangeRates(currencies, baseCurrency);
        
        // Assert
        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult, Is.Not.Null);
        Assert.That(objectResult.StatusCode, Is.GreaterThan(299));
        Assert.That(objectResult.Value, Contains.Substring($"Service Unavailable: Failed to retrieve exchange rates from "));
    }
}
