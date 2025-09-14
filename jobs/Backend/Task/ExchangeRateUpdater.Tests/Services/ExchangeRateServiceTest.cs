using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;
using ExchangeRateUpdater.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ExchangeRateUpdater.Tests.Services;

[TestFixture]
[TestOf(typeof(ExchangeRateService))]
public class ExchangeRateServiceTest
{
    private IExchangeRateProvider providerMock;
    private ExchangeRateService exchangeRateService;

    [SetUp]
    public void SetUp()
    {
        providerMock = Substitute.For<IExchangeRateProvider>();
        exchangeRateService = new ExchangeRateService(providerMock);
    }

    [TearDown]
    public void TearDown()
    {
        providerMock.ClearReceivedCalls();
    }

    [Test]
    public async Task GetRates_ReturnsRates_WhenProviderReturnsRates()
    {
        // Arrange
        var codes = new List<string> { "USD", "EUR" };
        var expected = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 23.5m),
            new(new Currency("EUR"), new Currency("CZK"), 25.0m)
        };
        providerMock.GetExchangeRates(Arg.Any<Currency[]>()).Returns(expected);

        // Act
        var result = await exchangeRateService.GetRates(codes);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
        await providerMock.Received(1).GetExchangeRates(Arg.Is<Currency[]>(arr => arr.Length == 2 && arr[0].Code == "USD" && arr[1].Code == "EUR"));
    }

    [Test]
    public void GetRates_ThrowsArgumentException_WhenCodesNullOrEmpty()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => exchangeRateService.GetRates(null));
        Assert.ThrowsAsync<ArgumentException>(() => exchangeRateService.GetRates(new List<string>()));
        providerMock.DidNotReceive().GetExchangeRates(Arg.Any<Currency[]>());
    }

    [Test]
    public void GetRates_PropagatesException_WhenProviderThrows()
    {
        // Arrange
        var codes = new List<string> { "USD" };
        providerMock.GetExchangeRates(Arg.Any<Currency[]>()).Throws(new InvalidOperationException());

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => exchangeRateService.GetRates(codes));
        providerMock.Received(1).GetExchangeRates(Arg.Any<Currency[]>());
    }
}