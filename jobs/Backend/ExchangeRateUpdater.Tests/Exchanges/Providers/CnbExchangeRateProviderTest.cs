using ExchangeRateUpdater.Exchanges.Providers;
using ExchangeRateUpdater.Model;
using ExchangeRateUpdater.Tests.TestUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace ExchangeRateUpdater.Tests.Exchanges.Providers;

public class CnbExchangeRateProviderTest
{
    [Fact]
    public void CnbExchangeRateProvider_MissingTargetCurrency()
    {
        var configurationData = new Dictionary<string, string>
        {
            { "PROVIDER", "CNB" },
            { "EXCHANGES:CNB_BASE_URL", "http://localhost" },
            //{ "EXCHANGES:CNB_TARGET_CURRENCY", "CZK" }
        };

        var config = new ConfigurationBuilder()
                        .AddInMemoryCollection(configurationData)
                        .Build();

        try
        {
            new CnbExchangeRateProvider(
                config, 
                new Mock<IHttpResilientClient>().Object, 
                new Mock<ILogger>().Object);
        }
        catch (InvalidOperationException ex)
        {
            Assert.Contains("EXCHANGES:CNB_TARGET_CURRENCY variable is not set", ex.Message);
            return;
        }

        Assert.Fail("Expected to throw InvalidOperationException when variable is missing");
    }

    [Fact]
    public async Task GetExchangeRates_ValidCurrencies()
    {
        var validResponseJson =
            """
            {
            "rates": [
              {
                "validFor": "2019-05-17",
                "order": 94,
                "country": "Australia",
                "currency": "dollar",
                "amount": 1,
                "currencyCode": "AUD",
                "rate": 15.858
              },
              {
                "validFor": "2019-05-17",
                "order": 94,
                "country": "Brazil",
                "currency": "real",
                "amount": 1,
                "currencyCode": "BRL",
                "rate": 5.686
              }
              ]
            }
            """;

        var validResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(validResponseJson, System.Text.Encoding.UTF8, "application/json")
        };

        var config = SetupUtils.GetTestConfig();
        var httpResilientClientMock = new Mock<IHttpResilientClient>();
        httpResilientClientMock.Setup(x => x.DoGet(It.IsAny<string>()))
                     .ReturnsAsync(validResponse);

        var provider = new CnbExchangeRateProvider(
                    config,
                    httpResilientClientMock.Object,
                    new Mock<ILogger>().Object);

        var currencies = new[]
        {
            new Currency("AUD"),
            new Currency("EUR"),
        };


        var rates = await provider.GetExchangeRates(currencies);

        Assert.Single(rates);
    }

    [Fact]
    public async Task GetExchangeRates_NoMatchingCurrencies()
    {
        var validResponseJson =
            """
            {
            "rates": [
              {
                "validFor": "2019-05-17",
                "order": 94,
                "country": "Australia",
                "currency": "dollar",
                "amount": 1,
                "currencyCode": "AUD",
                "rate": 15.858
              },
              {
                "validFor": "2019-05-17",
                "order": 94,
                "country": "Brazil",
                "currency": "real",
                "amount": 1,
                "currencyCode": "BRL",
                "rate": 5.686
              }
              ]
            }
            """;

        var validResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(validResponseJson, System.Text.Encoding.UTF8, "application/json")
        };

        var config = SetupUtils.GetTestConfig();
        var httpResilientClientMock = new Mock<IHttpResilientClient>();
        httpResilientClientMock.Setup(x => x.DoGet(It.IsAny<string>()))
                     .ReturnsAsync(validResponse);

        var provider = new CnbExchangeRateProvider(
                    config,
                    httpResilientClientMock.Object,
                    new Mock<ILogger>().Object);

        var currencies = new[]
        {
            new Currency("JPY"),
            new Currency("USD"),
        };


        var rates = await provider.GetExchangeRates(currencies);

        Assert.Empty(rates);
    }

    [Fact]
    public async Task GetExchangeRates_BadRequestHttpCall()
    {
        var config = SetupUtils.GetTestConfig();
        var badResponse = new HttpResponseMessage(HttpStatusCode.BadRequest) { };
        var httpResilientClientMock = new Mock<IHttpResilientClient>();
        httpResilientClientMock.Setup(x => x.DoGet(It.IsAny<string>()))
                     .ReturnsAsync(badResponse);

        var provider = new CnbExchangeRateProvider(
                    config,
                    httpResilientClientMock.Object,
                    new Mock<ILogger>().Object);


        try
        {
            var rates = await provider.GetExchangeRates(new List<Currency>());
        }
        catch(InvalidOperationException ex)
        {
            Assert.Contains("BadRequest", ex.Message);
        }
    }

    [Fact]
    public void CnbExchangeRateProvider_MissingUrl()
    {
        var configurationData = new Dictionary<string, string>
        {
            { "PROVIDER", "CNB" },
            //{ "EXCHANGES:CNB_BASE_URL", "http://localhost" },
            { "EXCHANGES:CNB_TARGET_CURRENCY", "CZK" }
        };

        var config = new ConfigurationBuilder()
                        .AddInMemoryCollection(configurationData)
                        .Build();

        try
        {
            new CnbExchangeRateProvider(
                config,
                new Mock<IHttpResilientClient>().Object,
                new Mock<ILogger>().Object);
        }
        catch (InvalidOperationException ex)
        {
            Assert.Contains("EXCHANGES:CNB_BASE_URL variable is not set", ex.Message);
            return;
        }

        Assert.Fail("Expected to throw InvalidOperationException when variable is missing");
    }

}
