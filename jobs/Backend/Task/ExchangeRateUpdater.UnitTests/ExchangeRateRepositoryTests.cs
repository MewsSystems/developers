using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.Data.Repositories;
using ExchangeRateUpdater.Infrastructure.HttpClients;
using Moq;
using NUnit.Framework;

namespace ExchangeRateUpdater.UnitTests;

[TestFixture]
public class ExchangeRateRepositoryTests
{
    private Mock<ICnbApiClient> _cnbApiClientMock;
    private CnbExchangeRateRepository _exchangeRateRepository;

    [SetUp]
    public void Setup()
    {
        _cnbApiClientMock = new Mock<ICnbApiClient>();
        _exchangeRateRepository = new CnbExchangeRateRepository(_cnbApiClientMock.Object);
    }

    [Test]
    public async Task GetTodayExchangeRatesAsync_WhenApiReturnsValidRates_ReturnsExchangeRates()
    {
        // Arrange
        var exchangeRates = new CnbExchangeRates
        {
            Rates =
                [
                    new() { CurrencyCode = "JPY", Rate = 23.084m, Amount = 1 },
                    new() { CurrencyCode = "EUR", Rate = 25.020m, Amount = 1 },
                    new() { CurrencyCode = "TRY", Rate = 0.68065m, Amount = 1 },
                ]
        };

        var expectedExchangeRates = exchangeRates.Rates.Select(r => new ExchangeRate(new Currency(r.CurrencyCode), new Currency("CZK"), r.Rate / r.Amount)).ToList();

        _cnbApiClientMock.Setup(a => a.GetTodayExchangeRatesAsync()).ReturnsAsync(exchangeRates);

        // Act
        var result = (await _exchangeRateRepository.GetTodayExchangeRatesAsync()).ToList();

        // Assert
        Assert.That(result, Has.Count.EqualTo(expectedExchangeRates.Count));
        Assert.That(expectedExchangeRates, Is.EquivalentTo(result));
    }

    [Test]
    public async Task GetExchangeRatesAsync_WhenDateIsPassed_ReturnsCorrectExchangeRates()
    {
        // Arrange
        var exchangeRates = new CnbExchangeRates
        {
            Rates =
                [
                    new() { CurrencyCode = "JPY", Rate = 23.084m, Amount = 1 },
                    new() { CurrencyCode = "EUR", Rate = 25.020m, Amount = 1 },
                    new() { CurrencyCode = "TRY", Rate = 0.68065m, Amount = 1 },
                ]
        };

        var expectedExchangeRates = exchangeRates.Rates.Select(r => new ExchangeRate(new Currency(r.CurrencyCode), new Currency("CZK"), r.Rate / r.Amount)).ToList();

        _cnbApiClientMock.Setup(a => a.GetExchangeRatesAsync(DateTime.Today)).ReturnsAsync(exchangeRates);

        // Act
        var result = (await _exchangeRateRepository.GetExchangeRatesAsync(DateTime.Today)).ToList();

        // Assert
        Assert.That(result, Has.Count.EqualTo(expectedExchangeRates.Count));
        Assert.That(expectedExchangeRates, Is.EquivalentTo(result));
    }

    [Test]
    public async Task GetTodayExchangeRatesAsync_WhenApiDoesNotReturnRates_ReturnsEmpty()
    {
        // Arrange
        var exchangeRates = new CnbExchangeRates { Rates = Array.Empty<CnbExchangeRate>() };
        _cnbApiClientMock.Setup(a => a.GetTodayExchangeRatesAsync()).ReturnsAsync(exchangeRates);

        // Act
        var result = await _exchangeRateRepository.GetTodayExchangeRatesAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }
}