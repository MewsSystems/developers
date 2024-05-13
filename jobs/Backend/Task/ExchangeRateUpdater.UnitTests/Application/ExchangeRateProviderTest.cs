using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.UnitTests.Application;

public class ExchangeRateProviderTest
{
    private readonly Mock<IExchangeRateApi> _exchangeRateApiMock;
    private readonly ExchangeRateProvider _exchangeRateProvider;

    public ExchangeRateProviderTest()
    {
        _exchangeRateApiMock = new Mock<IExchangeRateApi>();
        _exchangeRateProvider = new ExchangeRateProvider(_exchangeRateApiMock.Object);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenCurrenciesProvided_ShouldReturnFilteredRates()
    {
        var rates = new List<ExchangeRate>
        {
            new(new Currency("CZK"), new Currency("JPY"), 15.285m),
            new(new Currency("CZK"), new Currency("EUR"), 4.503m),
            new(new Currency("CZK"), new Currency("USD"), 12.750m),
            new(new Currency("CZK"), new Currency("ARS"), 16.910m),
            new(new Currency("CZK"), new Currency("XYZ"), 3.342m)
        };
        var currenciesFilter = new List<Currency>
        {
            new("JPY"),
            new("EUR"),
            new("USD"),
            new("XYZ")
        };

        _exchangeRateApiMock.Setup(x => x.GetExchangeRatesAsync(It.IsAny<DateOnly>(), It.IsAny<Language>())).ReturnsAsync(rates);

        var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(currenciesFilter, new DateOnly(2024, 5, 1), Language.EN);

        exchangeRates.Should()
            .NotBeNullOrEmpty()
            .And.HaveCount(4)
            .And.OnlyHaveUniqueItems();
        exchangeRates.Select(x => x.TargetCurrency).Should().BeEquivalentTo(currenciesFilter);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenCurrenciesProvided_ShouldReturnEmptyRatesList()
    {
        var currenciesFilter = new List<Currency>
        {
            new("JPY"),
            new("EUR"),
            new("USD"),
            new("XYZ")
        };
        _exchangeRateApiMock.Setup(x => x.GetExchangeRatesAsync(It.IsAny<DateOnly>(), It.IsAny<Language>()))
            .ReturnsAsync(Enumerable.Empty<ExchangeRate>());

        var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(currenciesFilter, new DateOnly(2024, 5, 1), Language.EN);

        exchangeRates.Should().BeEmpty();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenNoCurrenciesProvided_ShouldReturnEmptyRatesList()
    {
        _exchangeRateApiMock.Setup(x => x.GetExchangeRatesAsync(It.IsAny<DateOnly>(), It.IsAny<Language>()))
            .ReturnsAsync(Enumerable.Empty<ExchangeRate>());

        var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(Enumerable.Empty<Currency>(), new DateOnly(2024, 5, 1), Language.EN);

        exchangeRates.Should().BeEmpty();
    }
}
