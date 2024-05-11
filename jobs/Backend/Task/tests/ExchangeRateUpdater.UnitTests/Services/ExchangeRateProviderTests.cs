using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater.UnitTests.Services;

[TestFixture]
public class ExchangeRateProviderTests
{
    [SetUp]
    public void SetUp()
    {
        _client = new Mock<IExternalBankApiClient>();
        _sut = new ExchangeRateProvider(_client.Object);
    }

    private Mock<IExternalBankApiClient> _client = default!;
    private ExchangeRateProvider _sut = default!;

    [Test]
    public async Task GetExchangeRatesAsync_ShouldReturnExchangeRatesAmongSpecifiedCurrencies_WithSpecificCurrencies()
    {
        // arrange
        _client.Setup(m =>
                m.GetDailyExchangeRatesAsync(It.IsAny<DateOnly?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetDailyExchangeRatesResponse(new[]
            {
                new GetDailyExchangeRatesResponseItem(DateOnly.Parse("2024-05-10"), 90, "Australia", "dollar", 1, "AUD",
                    15.285m),
                new GetDailyExchangeRatesResponseItem(DateOnly.Parse("2024-05-10"), 90, "EMU", "euro", 1, "EUR",
                    24.935m),
                new GetDailyExchangeRatesResponseItem(DateOnly.Parse("2024-05-10"), 90, "USA", "dollar", 1, "USD",
                    23.131m)
            }));

        var currencies = new List<Currency>
        {
            new("USD"),
            new("EUR")
        };

        // act
        var result = await _sut.GetExchangeRatesAsync(currencies);
        var exchangeRates = result.ToList();

        // assert
        exchangeRates.Should().NotBeEmpty();
        exchangeRates.Should().HaveCount(2);
        exchangeRates[0].SourceCurrency.Should().BeEquivalentTo(new Currency("USD"));
        exchangeRates[0].TargetCurrency.Should().BeEquivalentTo(new Currency("CZK"));
        exchangeRates[0].Value.Should().Be(23.131m);
        exchangeRates[1].SourceCurrency.Should().BeEquivalentTo(new Currency("EUR"));
        exchangeRates[1].TargetCurrency.Should().BeEquivalentTo(new Currency("CZK"));
        exchangeRates[1].Value.Should().Be(24.935m);
    }
}