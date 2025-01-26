namespace ExchangeRateUpdater.Tests.CzechNationalBank;

public class UriBuilderTest
{
    [Fact]
    public void GivenUrl_WhenBuildingWithDate_ShouldBuildCorrectUrl()
    {
        var baseUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        var targetUrl = CzechNationalBankRateUriHelper.BuildMainSourceUri(new(baseUrl), new DateOnly(2025, 01, 25));

        targetUrl.ToString().Should().Be("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=25.01.2025");
    }
}