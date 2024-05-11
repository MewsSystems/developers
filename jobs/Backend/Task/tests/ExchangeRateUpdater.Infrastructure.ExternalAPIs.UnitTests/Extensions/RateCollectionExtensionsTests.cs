using AutoFixture;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Extensions;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Models;
using FluentAssertions;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.UnitTests.Extensions;

public class RateCollectionExtensionsTests
{
    [Fact]
    public void FilterAndConvertRates_Should_Only_Return_Provided_Currencies()
    {
        var fixture = new Fixture();

        CnbRate[] cnbRates = {
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "USD").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "GBP").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "JPY").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "CZH").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "AUD").Create(),
        };

        Currency[] filterCurrencies = new[]
        {
            new Currency("USD"),
            new Currency("CZH"),
            new Currency("Aud"), // Should work case-insensitively
        };
        
        var results = cnbRates.FilterAndConvertRates(filterCurrencies).ToArray();

        results.Should().HaveCount(3);
        results.Should().Contain(x => x.SourceCurrency.Code == "USD");
        results.Should().Contain(x => x.SourceCurrency.Code == "CZH");
        results.Should().Contain(x => x.SourceCurrency.Code == "AUD");
    }
    
    [Fact]
    public void FilterAndConvertRates_Should_Only_Return_Provided_Currencies_That_Exist()
    {
        var fixture = new Fixture();

        CnbRate[] cnbRates = {
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "USD").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "GBP").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "JPY").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "CZH").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "AUD").Create(),
        };

        Currency[] filterCurrencies = {
            new Currency("USD"),
            new Currency("CZH"),
            new Currency("Aud"), // Should work case-insensitively
            new Currency("TRY"), 
            new Currency("XYZ"), 
            new Currency("RUB"), 
        };
        
        var results = cnbRates.FilterAndConvertRates(filterCurrencies).ToArray();

        results.Should().HaveCount(3);
        results.Should().Contain(x => x.SourceCurrency.Code == "USD");
        results.Should().Contain(x => x.SourceCurrency.Code == "CZH");
        results.Should().Contain(x => x.SourceCurrency.Code == "AUD");
    }
    
    
    [Fact]
    public void FilterAndConvertRates_Should_Return_Empty_When_Rates_Is_Null()
    {
        Currency[] filterCurrencies = {
            new Currency("USD"),
            new Currency("CZH"),
            new Currency("AUD"),
        };
        
        IEnumerable<CnbRate>? cnbRates = null;

        var results = cnbRates.FilterAndConvertRates(filterCurrencies).ToArray();

        results.Should().HaveCount(0);
    }
}