using ExchangeRateUpdater.Tests.UnitTests.Helpers;
using FluentAssertions;

namespace ExchangeRateUpdater.Tests.UnitTests;

public class ExchangeRateProviderTests
{
    [Fact]
    public async void No_rates_are_calculated_when_currency_list_is_empty()
    {
        var czechExchangeRates = new StubExchangeRatesRepository();
        var sut = new ExchangeRateProvider(czechExchangeRates);

        var rates = await sut.GetExchangeRates([]);

        rates.Should().BeEmpty();
    }
    
    [Fact]
    public async void Retrieves_rates_for_each_pair_of_currencies_it_can()
    {
        var exchangeRates = new StubExchangeRatesRepository(
            CreateExchangeRate("NZD", "CZK", 1M),
            CreateExchangeRate("USD", "CZK", 2M),
            CreateExchangeRate("USD", "NZD", 0.8M));
        var sut = new ExchangeRateProvider(exchangeRates);
        List<Currency> currencies = [new Currency("USD"), new Currency("NZD"), new Currency("CZK")];
        
        var rates = await sut.GetExchangeRates(currencies);

        rates.Should()
            .ContainEquivalentOf(CreateExchangeRate("NZD", "CZK", 1M)).And
            .ContainEquivalentOf(CreateExchangeRate("USD", "CZK", 2M)).And
            .ContainEquivalentOf(CreateExchangeRate("USD", "NZD", 0.8M));
    }

    private static ExchangeRate CreateExchangeRate(string sourceCurrency, string targetCurrency, decimal value)
    {
        return new ExchangeRate(new Currency(sourceCurrency), new Currency(targetCurrency), value);
    }
}