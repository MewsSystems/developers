using ExchangeRateUpdater.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.UnitTests;

public class CnbDataParserTests
{
    private readonly Mock<ILogger<CnbDataParser>> _loggerMock;
    private readonly CnbDataParser _parser;

    public CnbDataParserTests()
    {
        _loggerMock = new Mock<ILogger<CnbDataParser>>();
        _parser = new CnbDataParser(_loggerMock.Object);
    }

    [Fact]
    public void Parse_ValidData_ReturnsExchangeRates()
    {
        // Arrange
        var rawData = @"05 Nov 2025 #215
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|14.567
USA|dollar|1|USD|22.950
EMU|euro|1|EUR|24.375";

        // Act
        var result = _parser.Parse(rawData).ToList();

        // Assert
        result.Should().HaveCount(3);

        result[0].Code.Should().Be("AUD");
        result[0].Amount.Should().Be(1);
        result[0].Rate.Should().Be(14.567m);

        result[1].Code.Should().Be("USD");
        result[2].Code.Should().Be("EUR");
    }

    [Fact]
    public void Parse_EmptyData_ReturnsEmptyCollection()
    {
        // Act
        var result = _parser.Parse(string.Empty);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Parse_NullData_ReturnsEmptyCollection()
    {
        // Act
        var result = _parser.Parse(null!);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Parse_DataWithDifferentAmounts_NormalizesCorrectly()
    {
        // Arrange
        var rawData = @"05 Nov 2025 #215
Country|Currency|Amount|Code|Rate
Japan|yen|100|JPY|15.234
Thailand|baht|1|THB|0.652";

        // Act
        var result = _parser.Parse(rawData).ToList();

        // Assert
        result.Should().HaveCount(2);
        result[0].Amount.Should().Be(100);
        result[1].Amount.Should().Be(1);
    }

    [Fact]
    public void Parse_MalformedLine_SkipsInvalidLine()
    {
        // Arrange
        var rawData = @"05 Nov 2025 #215
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|14.567
Invalid|Line|Data
USA|dollar|1|USD|22.950";

        // Act
        var result = _parser.Parse(rawData).ToList();

        // Assert
        result.Should().HaveCount(2);
        result[0].Code.Should().Be("AUD");
        result[1].Code.Should().Be("USD");
    }

    [Fact]
    public void Parse_InvalidNumericValues_SkipsInvalidLine()
    {
        // Arrange
        var rawData = @"05 Nov 2025 #215
Country|Currency|Amount|Code|Rate
Australia|dollar|INVALID|AUD|14.567
USA|dollar|1|USD|NOTANUMBER";

        // Act
        var result = _parser.Parse(rawData).ToList();

        // Assert
        result.Should().BeEmpty();
    }
}
