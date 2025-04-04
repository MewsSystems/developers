using FluentAssertions;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateParserTests
{
    [Fact]
    public void Parse_ValidData_ReturnsExchangeRates()
    {
        var input = @"
                03 Apr 2025 #66
                Country|Currency|Amount|Code|Rate
                Australia|dollar|1|AUD|14.326
                Brazil|real|1|BRL|4.017
                Bulgaria|lev|1|BGN|12.775
                Hungary|forint|100|HUF|6.219
                ";

        var result = ExchangeRateParser.Parse(input).ToList();

        result.Should().HaveCount(4);
        result[0].SourceCurrency.Code.Should().Be("CZK");
        result[0].TargetCurrency.Code.Should().Be("AUD");
        result[0].Value.Should().Be(14.326m);

        result[1].SourceCurrency.Code.Should().Be("CZK");
        result[1].TargetCurrency.Code.Should().Be("BRL");
        result[1].Value.Should().Be(4.017m);

        result[2].SourceCurrency.Code.Should().Be("CZK");
        result[2].TargetCurrency.Code.Should().Be("BGN");
        result[2].Value.Should().Be(12.775m);

        result[3].SourceCurrency.Code.Should().Be("CZK");
        result[3].TargetCurrency.Code.Should().Be("HUF");
        result[3].Value.Should().Be(0.06219m); // 100 HUF = 6.219 CZK, 1 HUF = 0.06219 CZK
    }

    [Fact]
    public void Parse_InvalidData_Ignored()
    {
        var input = @"
                03 Apr 2025 #66
                Country|Currency|Amount|Code|Rate
                Australia|dollar|abc|AUD|xyz
                Brazil|real|xyz|BRL|invalid
                ";

        var result = ExchangeRateParser.Parse(input);

        result.Should().BeEmpty();
    }

    [Fact]
    public void Parse_EmptyInput_ReturnsEmptyList()
    {
        var input = "";

        var result = ExchangeRateParser.Parse(input);

        result.Should().BeEmpty();
    }

    [Fact]
    public void Parse_SingleLine_ReturnsOneExchangeRate()
    {
        var input = @"
                03 Apr 2025 #66
                Country|Currency|Amount|Code|Rate
                Australia|dollar|1|AUD|14.326
                ";

        var result = ExchangeRateParser.Parse(input);

        result.Should().HaveCount(1);
    }
}
