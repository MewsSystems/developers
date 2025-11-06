using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;
using ExchangeRateUpdater.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ExchangeRateUpdater.Tests.Services;

[TestFixture]
[TestOf(typeof(ExchangeRateProvider))]
public class ExchangeRateProviderTest
{
    private IExchangeRatesClientStrategy clientStrategyMock;
    private ExchangeRateProvider provider;

    [SetUp]
    public void SetUp()
    {
        clientStrategyMock = Substitute.For<IExchangeRatesClientStrategy>();
        provider = new ExchangeRateProvider(clientStrategyMock);
    }

    [TearDown]
    public void TearDown()
    {
        clientStrategyMock.ClearReceivedCalls();
    }

    [Test]
    public async Task GetExchangeRates_ReturnsRatesForRequestedCurrencies()
    {
        // Arrange
        var requested = new[] { new Currency("USD"), new Currency("EUR") };
        var cnbRates = new List<CurrencyValue>
        {
            new()
            {
                CurrencyCode = "USD",
                Amount = 1,
                Rate = 23.5m,
                ValidFor = DateTime.Today
            },
            new()
            {
                CurrencyCode = "EUR",
                Amount = 1,
                Rate = 25.0m,
                ValidFor = DateTime.Today
            },
            new()
            {
                CurrencyCode = "JPY",
                Amount = 100,
                Rate = 17.5m,
                ValidFor = DateTime.Today
            }
        };
        clientStrategyMock.GetExchangeRates().Returns(cnbRates);

        // Act
        var result = (await provider.GetExchangeRates(requested)).ToList();

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Any(r => r.SourceCurrency.Code == "USD"));
        Assert.That(result.Any(r => r.SourceCurrency.Code == "EUR"));
        await clientStrategyMock.Received(1).GetExchangeRates();
    }

    [Test]
    public async Task GetExchangeRates_ReturnsEmpty_WhenNoRequestedCurrencyMatches()
    {
        // Arrange
        var requested = new[] { new Currency("GBP") };
        var cnbRates = new List<CurrencyValue>
        {
            new()
            {
                CurrencyCode = "USD",
                Amount = 1,
                Rate = 23.5m,
                ValidFor = DateTime.Today
            }
        };
        clientStrategyMock.GetExchangeRates().Returns(cnbRates);

        // Act
        var result = (await provider.GetExchangeRates(requested)).ToList();

        // Assert
        Assert.That(result, Is.Empty);
        await clientStrategyMock.Received(1).GetExchangeRates();
    }

    [Test]
    public void GetExchangeRates_PropagatesException_WhenClientThrows()
    {
        // Arrange
        var requested = new[] { new Currency("USD") };
        clientStrategyMock.GetExchangeRates().Throws(new InvalidOperationException());

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => provider.GetExchangeRates(requested));
        clientStrategyMock.Received(1).GetExchangeRates();
    }
}