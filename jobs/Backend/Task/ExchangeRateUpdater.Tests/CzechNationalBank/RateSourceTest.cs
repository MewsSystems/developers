using ExchangeRateUpdater.RateSources.CzechNationalBank;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Tests.CzechNationalBank;

public class RateSourceTest
{
    private readonly CzechNationalBankRateSource _rateSource;

    public RateSourceTest()
    {
        var http = new HttpClient();
        var httpFactory = Substitute.For<IHttpClientFactory>();
        httpFactory.CreateClient().ReturnsForAnyArgs(http);
        var parser = new CzechNationalBankRateParser();
        var uriBuilder = new CzechNationalBankRateUriBuilder(Options.Create(TestCommon.SourceOptions));
        _rateSource = new CzechNationalBankRateSource(
            parser,
            uriBuilder,
            NullCache.Instance,
            new CzechNationalBankRatesCacheExpirationCalculator(TimeProvider.System, new()),
            TimeProvider.System,
            NullLogger<CzechNationalBankRateSource>.Instance,
            httpFactory);
    }

    [Theory]
    [InlineData("2025.01.25", 15.118, 1, "AUD", "CZK")] // Primary source
    [InlineData("2025.01.25", 34.452, 100, "AFN", "CZK")] // Secondary source
    public async Task GivenDate_WhenCallingRealBankWebsite_ShouldReturnCorrectRates(string dateStr, decimal rate, decimal amount, string sourceCurrency, string targetCurrency)
    {
        var date = DateOnly.Parse(dateStr);
        var rates = await _rateSource.GetRatesAsync(date);

        rates.Should().NotBeEmpty();

        var exampleRate = rates.Single(x => x.SourceCurrency == new Currency(sourceCurrency));

        exampleRate.Rate.Should().Be(rate);
        exampleRate.Amount.Should().Be(amount);
        exampleRate.SourceCurrency.Code.Should().Be(sourceCurrency);
        exampleRate.TargetCurrency.Code.Should().Be(targetCurrency);
    }
}