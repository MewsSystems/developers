using ExchangeRateUpdater.CnbClient.Implementation;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Tests.CnbClient.Integration;

[TestFixture]
[Explicit("Runs against the real CNB API. Enable manually.")]
public class CnbClientIntegration
{
    private HttpExchangeRatesClientStrategy clientStrategy;

    [SetUp]
    public void SetUp()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.tests.json")
            .Build();

        var httpClientFactory = new TestHttpClientFactory();

        clientStrategy = new HttpExchangeRatesClientStrategy(httpClientFactory, configuration);
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnRates()
    {
        // Act
        var rates = await clientStrategy.GetExchangeRates();

        // Assert
        Assert.That(rates, Is.Not.Null);
        Assert.That(rates, Is.Not.Empty, "Expected at least one exchange rate");
    }
}