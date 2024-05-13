using AutoMapper;
using ExchangeRateUpdater.Application.MappingProfiles;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Infrastructure.Dtos;
using ExchangeRateUpdater.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.UnitTests.Application;

public class ExchangeRateProviderTest
{
    private readonly Mock<IExchangeRateApi> _exchangeRateApiMock;
    private readonly IMapper _mapper;

    private readonly ExchangeRateProvider _exchangeRateProvider;

    public ExchangeRateProviderTest()
    {
        _exchangeRateApiMock = new Mock<IExchangeRateApi>();
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(typeof(ExchangeRateProfile)));
        _mapper = mapperConfig.CreateMapper();

        _exchangeRateProvider = new ExchangeRateProvider(_exchangeRateApiMock.Object, _mapper);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenCurrenciesProvided_ShouldReturnFilteredRates()
    {
        var response = new List<CnbExchangeRateResponseItem>
        {
            new(1, "Japan", "yen", "JPY", 80, 15.285m, ""),
            new(1, "EMU", "euro", "EUR", 80, 4.503m, ""),
            new(1, "USA", "dollar", "USD", 80, 12.750m, ""),
            new(1, "Argentina", "pesos argentinos", "ARS", 80, 16.910m, ""),
            new(1, "Country", "XYZ", "XYZ", 80, 3.342m, "")
        };
        var currenciesFilter = new List<Currency>
        {
            new("JPY"),
            new("EUR"),
            new("USD"),
            new("XYZ")
        };
        var rates = new List<ExchangeRate>
        {
            new(new Currency("CZK"), new Currency("JPY"), 15.285m),
            new(new Currency("CZK"), new Currency("EUR"), 4.503m),
            new(new Currency("CZK"), new Currency("USD"), 12.750m),
            new(new Currency("CZK"), new Currency("XYZ"), 3.342m)
        };
        _exchangeRateApiMock
            .Setup(x => x.GetExchangeRatesAsync(It.IsAny<DateOnly>(), It.IsAny<Language>()))
            .ReturnsAsync(response);

        var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(currenciesFilter, new DateOnly(2024, 5, 1), Language.EN);

        exchangeRates.Should()
            .NotBeNullOrEmpty()
            .And.HaveCount(4)
            .And.OnlyHaveUniqueItems();
        exchangeRates.Select(x => x.TargetCurrency).Should().BeEquivalentTo(currenciesFilter);
        exchangeRates.Should().BeEquivalentTo(rates);
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
            .ReturnsAsync(Enumerable.Empty<CnbExchangeRateResponseItem>());

        var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(currenciesFilter, new DateOnly(2024, 5, 1), Language.EN);

        exchangeRates.Should().BeEmpty();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WhenNoCurrenciesProvided_ShouldReturnEmptyRatesList()
    {
        _exchangeRateApiMock.Setup(x => x.GetExchangeRatesAsync(It.IsAny<DateOnly>(), It.IsAny<Language>()))
            .ReturnsAsync(Enumerable.Empty<CnbExchangeRateResponseItem>());

        var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(Enumerable.Empty<Currency>(), new DateOnly(2024, 5, 1), Language.EN);

        exchangeRates.Should().BeEmpty();
    }
}
