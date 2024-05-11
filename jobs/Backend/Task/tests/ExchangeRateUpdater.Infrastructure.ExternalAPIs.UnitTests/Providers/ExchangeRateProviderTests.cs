using AutoFixture;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Clients;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Models;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Providers;
using FluentAssertions;
using NSubstitute;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.UnitTests.Providers;

public class ExchangeRateProviderTests
{
    private readonly ExchangeRateProvider _sut;
    private readonly IExchangeRateClient _client = Substitute.For<IExchangeRateClient>();
    public ExchangeRateProviderTests()
    {
        _sut = new ExchangeRateProvider(_client);
    }

    [Fact]
    public async Task GetExchangeRates_Returns_Expected_Rates()
    {
        var fixture = new Fixture();
        CnbRate[] cnbRates =
        {
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "USD").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "GBP").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "JPY").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "CZH").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "AUD").Create(),
        };
        Currency[] filterCurrencies =
        {
            new("USD"),
            new("CZH"),
            new("Aud"), // Should work case-insensitively
        };

        _client.GetRates(Arg.Any<CancellationToken>()).Returns(cnbRates);

        var result = await _sut.GetExchangeRates(filterCurrencies, CancellationToken.None);

        result.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task GetExchangeRates_Returns_Expected_Rates_When_Date_Provided()
    {
        var fixture = new Fixture();
        CnbRate[] cnbRates =
        {
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "USD").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "GBP").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "JPY").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "CZH").Create(),
            fixture.Build<CnbRate>().With(x => x.CurrencyCode, "AUD").Create(),
        };
        Currency[] filterCurrencies =
        {
            new("USD"),
            new("Aud"), // Should work case-insensitively
        };

        var date = new DateTime(2024, 5, 9);
        _client.GetRates(Arg.Is(date), Arg.Any<CancellationToken>()).Returns(cnbRates);

        var result = await _sut.GetExchangeRates(date, filterCurrencies, CancellationToken.None);
        result.Should().HaveCount(2);
    }
}