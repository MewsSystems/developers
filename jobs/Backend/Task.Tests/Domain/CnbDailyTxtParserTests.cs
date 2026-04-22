using ExchangeRateUpdater.Domain;
using Xunit;

namespace Task.Tests.Domain;

public sealed class CnbDailyTxtParserTests
{
    [Fact]
    public void Parse_valid_payload_returns_correct_rates()
    {
        string payload = """
            22.04.2026 #78
            Country|Currency|Amount|Code|Rate
            Australia|dollar|1|AUD|14,123
            United States|dollar|1|usd|22,640
            """;

        ExchangeRate[] rates = CnbDailyTxtParser.Parse(payload).ToArray();

        Assert.Equal(2, rates.Length);
        Assert.Equal("AUD", rates[0].SourceCurrency.Code);
        Assert.Equal("CZK", rates[0].TargetCurrency.Code);
        Assert.Equal(14.123m, rates[0].Value);
        Assert.Equal("USD", rates[1].SourceCurrency.Code);
        Assert.Equal(22.640m, rates[1].Value);
    }

    [Fact]
    public void Parse_amount_greater_than_one_normalizes_value()
    {
        string payload = """
            22.04.2026 #78
            Country|Currency|Amount|Code|Rate
            Japan|yen|100|JPY|15,320
            """;

        ExchangeRate rate = CnbDailyTxtParser.Parse(payload).Single();

        Assert.Equal("JPY", rate.SourceCurrency.Code);
        Assert.Equal(0.1532m, rate.Value);
    }

    [Fact]
    public void Parse_skips_header_lines_correctly()
    {
        string payload = """
            22.04.2026 #78
            This line is metadata
            Another metadata line
            Country|Currency|Amount|Code|Rate
            Sweden|krona|1|sek|2,078
            """;

        ExchangeRate rate = CnbDailyTxtParser.Parse(payload).Single();

        Assert.Equal("SEK", rate.SourceCurrency.Code);
        Assert.Equal("CZK", rate.TargetCurrency.Code);
        Assert.Equal(2.078m, rate.Value);
    }

    [Fact]
    public void Parse_accepts_dot_decimal_from_english_cnb_endpoint()
    {
        string payload = """
            22.04.2026 #78
            Country|Currency|Amount|Code|Rate
            Australia|dollar|1|AUD|14.795
            Japan|yen|100|JPY|15.320
            """;

        ExchangeRate[] rates = CnbDailyTxtParser.Parse(payload).ToArray();

        Assert.Equal(2, rates.Length);
        Assert.Equal("AUD", rates[0].SourceCurrency.Code);
        Assert.Equal(14.795m, rates[0].Value);
        Assert.Equal("JPY", rates[1].SourceCurrency.Code);
        Assert.Equal(0.1532m, rates[1].Value);
    }

    [Fact]
    public void Parse_malformed_line_throws_with_line_number()
    {
        string payload = """
            22.04.2026 #78
            Country|Currency|Amount|Code|Rate
            Australia|dollar|1|AUD
            """;

        FormatException ex = Assert.Throws<FormatException>(() => CnbDailyTxtParser.Parse(payload).ToArray());

        Assert.Contains("Malformed CNB line.", ex.Message);
        Assert.Contains("Line 3", ex.Message);
    }

    [Fact]
    public void Parse_invalid_decimal_throws()
    {
        string payload = """
            22.04.2026 #78
            Country|Currency|Amount|Code|Rate
            Australia|dollar|oops|AUD|14,123
            """;

        FormatException ex = Assert.Throws<FormatException>(() => CnbDailyTxtParser.Parse(payload).ToArray());

        Assert.Contains("Invalid amount in CNB line.", ex.Message);
        Assert.Contains("Line 3", ex.Message);
    }

    [Fact]
    public void Parse_missing_header_throws()
    {
        string payload = """
            22.04.2026 #78
            Metadata line
            Australia|dollar|1|AUD|14,123
            """;

        FormatException ex = Assert.Throws<FormatException>(() => CnbDailyTxtParser.Parse(payload).ToArray());

        Assert.Contains("Missing CNB header line.", ex.Message);
    }

    [Fact]
    public void Parse_rejects_zero_or_negative_amount()
    {
        string payload = """
            22.04.2026 #78
            Country|Currency|Amount|Code|Rate
            Australia|dollar|0|AUD|14,123
            """;

        FormatException ex = Assert.Throws<FormatException>(() => CnbDailyTxtParser.Parse(payload).ToArray());

        Assert.Contains("Amount must be greater than zero.", ex.Message);
        Assert.Contains("Line 3", ex.Message);
    }

    [Fact]
    public void Parse_rejects_zero_or_negative_rate()
    {
        string payload = """
            22.04.2026 #78
            Country|Currency|Amount|Code|Rate
            Australia|dollar|1|AUD|0
            """;

        FormatException ex = Assert.Throws<FormatException>(() => CnbDailyTxtParser.Parse(payload).ToArray());

        Assert.Contains("Rate must be greater than zero.", ex.Message);
        Assert.Contains("Line 3", ex.Message);
    }

    [Fact]
    public void Parse_malformed_line_longer_than_100_chars_is_truncated_in_error()
    {
        string longCountryName = new('A', 101);
        string payload = $"""
            22.04.2026 #78
            Country|Currency|Amount|Code|Rate
            {longCountryName}|dollar|1|AUD
            """;

        FormatException ex = Assert.Throws<FormatException>(() => CnbDailyTxtParser.Parse(payload).ToArray());

        Assert.Contains("...", ex.Message);
        Assert.DoesNotContain(longCountryName, ex.Message);
    }
}
