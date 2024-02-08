using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Tests.Fixtures;
using FluentAssertions;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderUnitTests : IClassFixture<ExchangeRateData>
{
    private readonly ExchangeRateData _fixture;

    public ExchangeRateProviderUnitTests(ExchangeRateData exchangeRateData)
    {
        _fixture = exchangeRateData;
    }

    [Fact]
    public async Task Given_NullCurrencyList_When_GetExchangeRates_Then_ReturnNoRates()
    {
        var sut = new ExchangeRateProvider(_fixture.MockCnbClient);

        var rates = await sut.GetExchangeRates(null);

        rates.Should().BeEmpty();
    }

    [Fact]
    public async Task Given_CurrenciesNotInFeed_When_GetExchangeRates_Then_ReturnsNoRates()
    {
        var sut = new ExchangeRateProvider(_fixture.MockCnbClient);

        var rates = await sut.GetExchangeRates(_fixture.CurrenciesNotInTestData);

        rates.Should().BeEmpty();
    }

    [Fact]
    public async Task Given_CurrenciesInFeed_When_GetExchangeRates_Then_ReturnsSelectedRates()
    {
        var sut = new ExchangeRateProvider(_fixture.MockCnbClient);

        //Get the currencies that start with C
        var currencies = _fixture.CurrenciesInTestData.Where(c => c.Code.StartsWith("C")).ToList();

        var rates = await sut.GetExchangeRates(currencies);

        rates.Should().HaveCount(currencies.Count);
    }

    [Fact]
    public async Task GivenMixOfCurrenciesInFeedAndCurrenciesNotInFeed_Then_ReturnsRatesForThoseCurrenciesInFeed()
    {
        var sut = new ExchangeRateProvider(_fixture.MockCnbClient);

        //Get the currencies that start with B or C
        var currencies = _fixture.CurrenciesInTestData.Where(c => c.Code.StartsWith("B") || c.Code.StartsWith("C")).ToList();

        var expectedCount = currencies.Count;

        currencies.AddRange(new[] { new Currency("CYZ"), new Currency("BAC") });

        var rates = await sut.GetExchangeRates(currencies);

        rates.Should().HaveCount(expectedCount);
    }
}