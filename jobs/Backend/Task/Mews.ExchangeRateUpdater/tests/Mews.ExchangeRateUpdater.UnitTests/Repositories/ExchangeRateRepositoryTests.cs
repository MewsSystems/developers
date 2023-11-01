using Mews.ExchangeRateUpdater.Domain.Entities;
using Mews.ExchangeRateUpdater.Domain.Entities.ExchangeRateAgg;
using Mews.ExchangeRateUpdater.Infrastructure.Data.Repositories;
using Mews.ExchangeRateUpdater.Infrastructure.HttpClients;
using Microsoft.Extensions.Caching.Memory;

namespace Mews.ExchangeRateUpdater.UnitTests.Repositories;

/// <summary>
/// Contains unit tests for the <see cref="ExchangeRateRepository"/> class.
/// </summary>
[TestFixture]
public class ExchangeRateRepositoryTests
{
    private Mock<IMemoryCache> _mockCache = null!;
    private Mock<ICzechNationalBankApiClient> _mockCzechNationalBankApiClient = null!;
    private ExchangeRateRepository _exchangeRateRepository = null!;

    /// <summary>
    /// Initializes mock objects and the <see cref="ExchangeRateRepository"/> instance before each test is run.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _mockCache = new Mock<IMemoryCache>();
        _mockCzechNationalBankApiClient = new Mock<ICzechNationalBankApiClient>();
        _exchangeRateRepository = new ExchangeRateRepository(_mockCzechNationalBankApiClient.Object, _mockCache.Object);
    }

    /// <summary>
    /// Verifies that the method <see cref="ExchangeRateRepository.GetTodayExchangeRatesAsync"/> returns a correct list of exchange rates when the external API provides valid data.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task GetTodayExchangeRatesAsync_WhenApiReturnsRates_ShouldReturnCorrectExchangeRates()
    {
        // Arrange
        var bankRates = new CNBExchangeRates
        {
            Rates = new List<CNBExchangeRate>
                {
                    new() { CurrencyCode = "USD", Rate = 23.084m, Amount = 1 },
                    new() { CurrencyCode = "EUR", Rate = 25.75m, Amount = 1 },
                    new() { CurrencyCode = "CAD", Rate = 17.081m, Amount = 1 },
                    new() { CurrencyCode = "NZD", Rate = 15.051m, Amount = 1 }
                }
        };

        var expectedExchangeRates = bankRates.Rates.Select(rate =>
            new ExchangeRate(new Currency(rate.CurrencyCode), new Currency("CZK"),
            decimal.Divide(rate.Rate, rate.Amount))).ToList();

        _mockCzechNationalBankApiClient.Setup(api => api.GetTodayExchangeRatesAsync())
            .ReturnsAsync(bankRates);

        // Act
        var result = (await _exchangeRateRepository.GetTodayExchangeRatesAsync()).ToList();

        // Assert
        Assert.That(result, Has.Count.EqualTo(expectedExchangeRates.Count));
        CollectionAssert.AreEqual(expectedExchangeRates, result.ToList());
    }

    /// <summary>
    /// Verifies that the method <see cref="ExchangeRateRepository.GetTodayExchangeRatesAsync"/> returns an empty list when the external API provides no exchange rate data.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task GetTodayExchangeRatesAsync_WhenApiReturnsNoRates_ShouldReturnEmptyList()
    {
        // Arrange
        var bankRates = new CNBExchangeRates { Rates = new List<CNBExchangeRate>() };
        _mockCzechNationalBankApiClient.Setup(api => api.GetTodayExchangeRatesAsync())
            .ReturnsAsync(bankRates);

        // Act
        var result = await _exchangeRateRepository.GetTodayExchangeRatesAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }
}