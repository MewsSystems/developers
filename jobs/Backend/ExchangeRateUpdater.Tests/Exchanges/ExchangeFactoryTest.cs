using ExchangeRateUpdater.Exchanges;
using ExchangeRateUpdater.Tests.TestUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests.Exchanges;

public class ExchangeFactoryTest
{
    [Fact]
    public void GetExchangeRateProvider_ValidConfig()
    {
        var config = SetupUtils.GetTestConfig();

        var provider = ExchangeFactory.GetExchangeRateProvider(
            config, 
            new Mock<IHttpResilientClient>().Object, 
            new Mock<ILogger>().Object);

        Assert.NotNull(provider);
    }

    [Fact]
    public void GetExchangeRateProvider_MissingProviderConfig()
    {
        var configurationData = new Dictionary<string, string>
        {
            //{ "PROVIDER", "CNB" },
            { "EXCHANGES:CNB_BASE_URL", "http://localhost" },
            { "EXCHANGES:CNB_TARGET_CURRENCY", "CZK" }
        };

        var config = new ConfigurationBuilder()
                        .AddInMemoryCollection(configurationData)
                        .Build();

        try
        {
            var provider = ExchangeFactory.GetExchangeRateProvider(
                config, 
                new Mock<IHttpResilientClient>().Object, 
                new Mock<ILogger>().Object);
        }
        catch(InvalidOperationException ex)
        {
            Assert.Contains("Exchange rate provider not set", ex.Message);
            return;
        }

        Assert.Fail("Expected to throw InvalidOperationException");
    }

    [Fact]
    public void GetExchangeRateProvider_UndefinedProvider()
    {
        var configurationData = new Dictionary<string, string>
        {
            { "PROVIDER", "undefined" },
            { "EXCHANGES:CNB_BASE_URL", "http://localhost" },
            { "EXCHANGES:CNB_TARGET_CURRENCY", "CZK" }
        };

        var config = new ConfigurationBuilder()
                        .AddInMemoryCollection(configurationData)
                        .Build();

        try
        {
            var provider = ExchangeFactory.GetExchangeRateProvider(
                config, 
                new Mock<IHttpResilientClient>().Object, 
                new Mock<ILogger>().Object);
        }
        catch (InvalidOperationException ex)
        {
            Assert.Contains("Invalid exchange rate provider", ex.Message);
            return;
        }

        Assert.Fail("Expected to throw InvalidOperationException");
    }
}
