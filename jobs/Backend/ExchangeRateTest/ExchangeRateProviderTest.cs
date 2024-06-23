using ExchangeRateUpdater;
using Moq;
using Moq.Protected;

namespace ExchangeRateTest;

public class ExchangeRateProviderTest
{
    private readonly Mock<CZKExchangeRateApiProvider> _mockApiProvider;
    private readonly Mock<ExchangeRateProvider> _mockExchangeProvider;

    public ExchangeRateProviderTest()
    {
        var usd_dictionary = new Dictionary<string, decimal>
        {
            { "CZK", (decimal)23.341 },
            { "EUR", (decimal)0.93 }
        };
        var eur_dictionary = new Dictionary<string, decimal>
        {
            { "CZK", (decimal)24.945 }
        };
        var exchangeDictionary = new Dictionary<string, Dictionary<string, decimal>>
        {
            { "USD", usd_dictionary },
            { "EUR", eur_dictionary }
        };

        _mockApiProvider = new Mock<CZKExchangeRateApiProvider>();
        _mockApiProvider.Setup(m => m.FetchRates()).ReturnsAsync(
            new ExchangeRatesDictionary(exchangeDictionary)
        );
        
        _mockExchangeProvider = new Mock<ExchangeRateProvider>() { CallBase = true };
        _mockExchangeProvider.Protected().Setup<CZKExchangeRateApiProvider>("CreateApiProvider").Returns(
            _mockApiProvider.Object
        );
    }

    [Fact]
    public void WhenAsksForAnExistingCurrency()
    {
        var exchangeRateProvider = _mockExchangeProvider.Object;
        var currencies = new[]
        {
            new Currency("USD")
        };

        var result = exchangeRateProvider.GetExchangeRates(currencies);

        Assert.Single(result);
        Assert.Equal((decimal)23.341, result.First().Value);
    }

    [Fact]
    public void WhenAsksForANonExistingCurrency()
    {
        var exchangeRateProvider = _mockExchangeProvider.Object;
        var currencies = new[]
        {
            new Currency("XYZ")
        };

        var result = exchangeRateProvider.GetExchangeRates(currencies);

        Assert.Empty(result);
    }

    [Fact]
    public void WhenAsksForMultipleCurrencies()
    {
        var exchangeRateProvider = _mockExchangeProvider.Object;
        var currencies = new[]
        {
            new Currency("EUR"),
            new Currency("USD"),
            new Currency("JPY")
        };

        var result = exchangeRateProvider.GetExchangeRates(currencies);

        Assert.Equal(2, result.Count());
    }
}