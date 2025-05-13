using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Providers;
using NSubstitute;
using Shouldly;

namespace ExchangeRateUpdater.Tests.Providers;

public class ExchangeRateProviderTests
{
    private readonly IExchangeRateRepository _exchangeRateRepository;
    public ExchangeRateProviderTests()
    {
        _exchangeRateRepository = Substitute.For<IExchangeRateRepository>();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ShouldReturnFilteredRates_WhenCurrenciesMatch()
    {
        // Arrange
        IEnumerable<Currency> currencies =
        [
            new Currency("USD"),
            new Currency("EUR")
        ];

        ExchangeRate filterOutExchangeRate = new(new Currency("GBP"), new Currency("CZK"), 30.5m);

        IEnumerable<ExchangeRate> repositoryExchangeRates =
        [
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.5m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 27.0m),
            filterOutExchangeRate
        ];

        _exchangeRateRepository.GetCzkExchangeRatesAsync().Returns(repositoryExchangeRates);
        ExchangeRateProvider sut = new(_exchangeRateRepository);

        // Act
        IEnumerable<ExchangeRate> filteredExchangeRates = await sut.GetExchangeRatesAsync(currencies);

        // Assert
        filteredExchangeRates.Count().ShouldBe(2);
        filteredExchangeRates.Any(x => x.SourceCurrency.Code == "USD").ShouldBeTrue();
        filteredExchangeRates.Any(x => x.SourceCurrency.Code == "EUR").ShouldBeTrue();
        filteredExchangeRates.Any(x => x.SourceCurrency.Code == filterOutExchangeRate.SourceCurrency.Code).ShouldBeFalse();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ShouldReturnEmpty_WhenNoRatesMatch()
    {
        // Arrange
        IEnumerable<Currency> currencies =
        [
            new Currency("AUD"),
            new Currency("CAD")
        ];

        IEnumerable<ExchangeRate> repositoryExchangeRates =
        [
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.5m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 27.0m),
        ];

        _exchangeRateRepository.GetCzkExchangeRatesAsync().Returns(repositoryExchangeRates);
        ExchangeRateProvider sut = new(_exchangeRateRepository);

        // Act
        IEnumerable<ExchangeRate> filteredExchangeRates = await sut.GetExchangeRatesAsync(currencies);

        // Assert
        filteredExchangeRates.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ShouldReturnFilteredRates_WhenSomeCurrenciesAreMissing()
    {
        // Arrange
        Currency notExistingCurrency = new("JPY");
        Currency existingCurrency = new("USD");
        IEnumerable<Currency> currencies =
        [
            existingCurrency,
            notExistingCurrency
        ];

        ExchangeRate filteredOutExchangeRate = new(new Currency("EUR"), new Currency("CZK"), 27.0m);
        IEnumerable<ExchangeRate> repositoryExchangeRates =
        [
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.5m),
            filteredOutExchangeRate
        ];

        _exchangeRateRepository.GetCzkExchangeRatesAsync().Returns(repositoryExchangeRates);
        ExchangeRateProvider sut = new(_exchangeRateRepository);

        // Act
        IEnumerable<ExchangeRate> filteredExchangeRates = await sut.GetExchangeRatesAsync(currencies);

        // Assert
        filteredExchangeRates.Count().ShouldBe(1);
        filteredExchangeRates.Any(x => x.SourceCurrency.Code == existingCurrency.Code).ShouldBeTrue();
        filteredExchangeRates.Any(x => x.SourceCurrency.Code == filteredOutExchangeRate.SourceCurrency.Code).ShouldBeFalse();
        filteredExchangeRates.Any(x => x.SourceCurrency.Code == notExistingCurrency.Code).ShouldBeFalse();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ShouldReturnEmpty_WhenRepositoryReturnsNoRates()
    {
        // Arrange
        IEnumerable<Currency> currencies =
        [
            new ("USD")
        ];

        _exchangeRateRepository.GetCzkExchangeRatesAsync().Returns([]);
        ExchangeRateProvider sut = new(_exchangeRateRepository);

        // Act
        IEnumerable<ExchangeRate> filteredExchangeRates = await sut.GetExchangeRatesAsync(currencies);

        // Assert
        filteredExchangeRates.ShouldBeEmpty();
    }
}
