using ExchangeRateUpdater.RateSources.CzechNationalBank;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Tests.CzechNationalBank;

public class UriBuilderTest
{
    private readonly CzechNationalBankRateUriBuilder _uriBuilder;

    public UriBuilderTest()
    {
        _uriBuilder = new CzechNationalBankRateUriBuilder(Options.Create(TestCommon.SourceOptions));
    }
    [Fact]
    public void GivenUrl_WhenBuildingWithDate_ShouldBuildCorrectUrl()
    {
        var targetUrl = _uriBuilder.BuildMainSourceUri(new DateOnly(2025, 01, 25));
        targetUrl.ToString().Should().Be("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=25.01.2025");
    }

    [Fact]
    public void GivenSecondarySourceUrl_WhenBuildingWithDate_ShouldBuildCorrectUrl()
    {
        var targetUrl = _uriBuilder.BuildSecondarySourceUri(new DateOnly(2025, 01, 25));

        targetUrl.ToString().Should().Be("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?year=2025&month=1");
    }
}