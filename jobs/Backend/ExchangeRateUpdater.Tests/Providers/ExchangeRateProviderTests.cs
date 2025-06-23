using ExchangeRateUpdater;
using ExchangeRateUpdater.Models;

[TestFixture]
public class ExchangeRateProviderTests
{
    private ExchangeRateProvider _provider;

    [SetUp]
    public void SetUp()
    {
        _provider = new ExchangeRateProvider();
    }

    [Test]
    public void GetExchangeRates_ShouldReturnEmpty_WhenNoCurrenciesProvided()
    {
        // Arrange
        var currencies = new List<Currency>();

        // Act
        var result = _provider.GetExchangeRates(currencies);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }

    [Test]
    public void GetExchangeRates_ShouldNotThrowException_WhenCalledWithValidCurrencies()
    {
        // Arrange
        var currencies = new List<Currency>
        {
            new Currency("USD"),
            new Currency("EUR")
        };

        // Act & Assert
        Assert.DoesNotThrow(() => _provider.GetExchangeRates(currencies));
    }
}