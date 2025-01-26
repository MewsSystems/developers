using ExchangeRateUpdater.Sources.CzechNationalBank;

namespace ExchangeRateUpdater.Tests.CzechNationalBank;

public class RateSourceTest
{
    private readonly CzechNationalBankRateSource _rateSource;

    public RateSourceTest()
    {
        var parser = new CzechNationalBankRateParser();
        _rateSource = new CzechNationalBankRateSource(parser, new HttpClient());
    }

    [Theory(Skip = "Real external website call.")]
    [InlineData("2025.01.25", 0, 15.118, 1, "AUD", "CZK")]
    [InlineData("2025.01.20", 0, 15.161, 1, "AUD", "CZK")]
    public async Task GivenDate_WhenCallingRealBankWebsite_ShouldReturnCorrectRates(string dateStr, int rateIndex, decimal rate, decimal amount, string sourceCurrency, string targetCurrency)
    {
        var date = DateOnly.Parse(dateStr);
        var rates = await _rateSource.GetRatesAsync(date);

        rates.Should().NotBeEmpty();

        var exampleRate = rates[rateIndex];
        
        exampleRate.Rate.Should().Be(rate);
        exampleRate.Amount.Should().Be(amount);
        exampleRate.SourceCurrency.Code.Should().Be(sourceCurrency);
        exampleRate.TargetCurrency.Code.Should().Be(targetCurrency);
    }
}