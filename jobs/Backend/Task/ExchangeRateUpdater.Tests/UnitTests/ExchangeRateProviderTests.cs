using FluentAssertions;

namespace ExchangeRateUpdater.Tests.UnitTests;

public class ExchangeRateProviderTests
{
    [Fact]
    public void no_rates_are_calculated_when_currency_list_is_empty()
    {
        var sut = new ExchangeRateProvider();

        var rates = sut.GetExchangeRates([]);

        rates.Should().BeEmpty();
    }
}