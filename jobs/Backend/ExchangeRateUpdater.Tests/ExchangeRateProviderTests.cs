using System.Linq;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class ExchangeRateProviderTests
{

    private Mock<IExchangeRateClient> _mockExchangeRateClient;
    private Mock<IExchangeRateSource> _mockExchangeRateSource;

    [SetUp]
    public void Setup()
    {
        _mockExchangeRateClient = new Mock<IExchangeRateClient>();
        _mockExchangeRateSource = new Mock<IExchangeRateSource>();
    }

    [Test]
    public async Task CnbExchangeRateProvider_ReturnsOnlyRequiredExchangeRates()
    {
        // Arrange
        string mockExchangeRateClientResult = @"{""rates"":[
                {
                ""currencyCode"":""USD"",
                ""rate"":123.0
                },
                {
                ""currencyCode"":""EUR"",
                ""rate"":321.0
                },
                {
                ""currencyCode"":""FAKE"",
                ""rate"":999.0
                }
            ]}";
        _mockExchangeRateClient.Setup(e => e.GetExchangeRateAsync()).ReturnsAsync(mockExchangeRateClientResult);
        _mockExchangeRateSource.SetupGet(e => e.CurrencyCode).Returns(new Currency("TEST"));
        var exchangeRateProvider =
            new CnbExchangeRateProvider(_mockExchangeRateClient.Object, _mockExchangeRateSource.Object);

        IEnumerable<Currency> currencies = new[]
        {
            new Currency("USD"),
            new Currency("TEST"),
            new Currency("EUR")
        };

        // Act
        var results = exchangeRateProvider.GetExchangeRates(currencies);
        var resultsList = new List<ExchangeRate>();
        
        await foreach (var exchangeRate in results)
        {
            resultsList.Add(exchangeRate);
        }

        // Assert
        Assert.That(resultsList.All(result => currencies.Any(c => c.Code == result.SourceCurrencyCode)), Is.True); // Checks if any exchange rates that aren't in the provided list are returned

    }
}