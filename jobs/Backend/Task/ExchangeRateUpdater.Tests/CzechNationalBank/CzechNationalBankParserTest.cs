using ExchangeRateUpdater.Sources.CzechNationalBank;

namespace ExchangeRateUpdater.Tests.CzechNationalBank;

public class CzechNationalBankParserTest
{
    private readonly CzechNationalBankRateParser _parser;

    public CzechNationalBankParserTest()
    {
        _parser = new CzechNationalBankRateParser();
    }
    [Theory]
    [InlineData(0, 15.118, 1, "AUD", "CZK")]
    [InlineData(1, 4.062, 1, "BRL", "CZK")]
    [InlineData(8, 6.137, 100, "HUF", "CZK")]
    public void GivenExampleValidRate_WhenParsing_ShouldReturnCorrectResult(int rateIndex, decimal rate, decimal amount, string sourceCurrency, string targetCurrency)
    {
        var rates = _parser.Parse(ExampleRates.CzechNationalBank).ToList();

        var exampleRate = rates[rateIndex];

        exampleRate.Rate.Should().Be(rate);
        exampleRate.Amount.Should().Be(amount);
        exampleRate.SourceCurrency.Code.Should().Be(sourceCurrency);
        exampleRate.TargetCurrency.Code.Should().Be(targetCurrency);
    }

    [Fact]
    public void GivenIncompleteRateLine_WhenParsing_ShouldThrowException()
    {
        var parseFunc = () => _parser.Parse(ExampleRates.InvalidCzechNationalBank).ToList();
        parseFunc.Should().Throw<CzechNationalBankRateParserException>();
    }
}