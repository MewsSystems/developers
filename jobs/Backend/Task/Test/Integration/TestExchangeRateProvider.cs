using ExchangeRateUpdater.ExchangeApis.CnbApi;
using ExchangeRateUpdater.RateProvider;
using FluentAssertions;

namespace Test.Integration;

public class TestExchangeRateProvider
{
    private readonly static HttpClient _httpClient = new() { BaseAddress = new Uri("https://api.cnb.cz/cnbapi/") };

    [Fact]
    public async Task WhenGettingExchangeRates_ReturnsFilteredRates()
    {
        // Given
        var api = new CnbApi(_httpClient);
        var provider = new ExchangeRateProvider(api);
        var currencies = new[]
        {
            new Currency("EUR"),
            new Currency("LKR"),
            new Currency("NON_EXISTING"),
        };

        // When
        var rates = await provider.GetExchangeRates(currencies);

        // Then
        rates.Should().HaveCount(2);
        rates.Should().ContainSingle(r => r.SourceCurrency.Code == "EUR");
        rates.Should().ContainSingle(r => r.SourceCurrency.Code == "LKR");
        rates.Should().OnlyContain(r => r.TargetCurrency.Code == "CZK");
        rates.Should().OnlyContain(r => r.Value > 0);
        rates.Should().NotContain(r => r.SourceCurrency.Code == "NON_EXISTING");
    }
}