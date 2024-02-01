using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Idioms;
using Mews.ExchangeRate.Domain.UnitTests.FixtureCustomizations;
using NSubstitute;

namespace Mews.ExchangeRate.Domain.UnitTests;
public class ExchangeRateProviderTests
{
    private Fixture _fixture;

    public ExchangeRateProviderTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        _fixture.CustomizeWithEURCurrencyAndExchangeRate();
        var ratesRetrieverMock = _fixture.Freeze<IRetrieveExchangeRatesFromSource>();
        ratesRetrieverMock.GetAllExchangeRatesAsync()
            .Returns(new List<ExchangeRate>()
            {
                _fixture.Create<ExchangeRate>()
            });
    }

    [Fact]
    public void Constructor_isGuardedAgainst_nulls()
    {
        var assertion = _fixture.Create<GuardClauseAssertion>();
        assertion.Verify(typeof(Currency).GetConstructors());
    }

    [Fact]
    public async Task GetExchangeRatesForCurrenciesAsync_ReturnsExchangeRatesForEUR_WhenRateIsReturnedFromSource()
    {
        var eurCurrency = _fixture.Create<Currency>();
        var currencyList = new List<Currency>()
        {
            eurCurrency
        };

        var sut = _fixture.Create<ExchangeRateProvider>();
        var result = await sut.GetExchangeRatesForCurrenciesAsync(currencyList);

        result.All(x => x.TargetCurrency.Code.Equals(eurCurrency.Code) || x.SourceCurrency.Code.Equals(eurCurrency.Code));
    }

    [Fact]
    public async Task GetExchangeRatesForCurrenciesAsync_IgnoresCurrencies_WhenSourceDoesNotReturnExchangeRates()
    {
        var eurCurrency = new Currency("USD");
        var currencyWithNoExchangeRates = new Currency("USD");
        var currencyList = new List<Currency>()
        {
            eurCurrency,
            currencyWithNoExchangeRates
        };

        var sut = _fixture.Create<ExchangeRateProvider>();
        var result = await sut.GetExchangeRatesForCurrenciesAsync(currencyList);

        result.All(x => x.TargetCurrency.Code.Equals(eurCurrency.Code) || x.SourceCurrency.Code.Equals(eurCurrency.Code));
    }

    //OTHER TESTS 
    //public async Task GetExchangeRatesForCurrenciesAsync_ReturnsEmptyCollection_WhenNoCurrencyHasExchangeRates()
    //public async Task GetExchangeRatesForCurrenciesAsync_ThrowsException_WhenSourceThrowsException()
    //public async Task GetExchangeRatesForCurrenciesAsync_isGuardedAgainst_nulls()
}
