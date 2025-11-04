using ExchangeRateUpdater.Errors;
using ExchangeRateUpdater.Services.Parsers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests.Parsers;

public class CnbDataParserTests
{
    private readonly CnbDataParser _parser;
    private readonly Mock<ILogger<CnbDataParser>> _loggerMock;

    public CnbDataParserTests()
    {
        _loggerMock = new Mock<ILogger<CnbDataParser>>();
        _parser = new CnbDataParser(_loggerMock.Object);
    }

    [Fact]
    public void Parse_WithValidData_ReturnsSuccessResult()
    {
        // Arrange
        var validData = @"19 Aug 2025 #159
                         Country|Currency|Amount|Code|Rate
                         Australia|dollar|1|AUD|14.165
                         Brazil|real|1|BRL|3.745
                         EMU|euro|1|EUR|24.455
                         Japan|yen|100|JPY|14.165";

        // Act
        var result = _parser.Parse(validData);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Date.Should().Be(new DateTime(2025, 8, 19));
        result.Value.Rates.Should().HaveCount(4);

        var eurRate = result.Value.Rates.Find(r => r.Code == "EUR");
        eurRate.Should().NotBeNull();
        eurRate.Rate.Should().Be(24.455m);
        eurRate.Amount.Should().Be(1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_WithEmptyOrNullInput_ReturnsEmptyResponseError(string input)
    {
        // Act
        var result = _parser.Parse(input);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Metadata.Should().ContainKey("ErrorCode");
        result.Errors[0].Metadata["ErrorCode"].Should().Be(CnbErrorCode.EmptyResponse);
    }

    [Fact]
    public void Parse_WithInsufficientLines_ReturnsInsufficientDataError()
    {
        // Arrange
        var insufficientData = "19 Aug 2025 #159";

        // Act
        var result = _parser.Parse(insufficientData);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Metadata["ErrorCode"].Should().Be(CnbErrorCode.InsufficientData);
    }

    [Fact]
    public void Parse_WithInvalidDateFormat_ReturnsInvalidDateFormatError()
    {
        // Arrange
        var invalidDateData = @"Invalid Date Format
                               Country|Currency|Amount|Code|Rate
                               Australia|dollar|1|AUD|14.165";

        // Act
        var result = _parser.Parse(invalidDateData);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Metadata["ErrorCode"].Should().Be(CnbErrorCode.InvalidDateFormat);
    }

    [Fact]
    public void Parse_WithInvalidHeader_ReturnsInvalidHeaderFormatError()
    {
        // Arrange
        var invalidHeaderData = @"19 Aug 2025 #159
                                 Invalid|Header|Format
                                 Australia|dollar|1|AUD|14.165";

        // Act
        var result = _parser.Parse(invalidHeaderData);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Metadata["ErrorCode"].Should().Be(CnbErrorCode.InvalidHeaderFormat);
    }

    [Fact]
    public void Parse_WithNoValidRates_ReturnsNoValidRatesError()
    {
        // Arrange
        var noValidRatesData = @"19 Aug 2025 #159
                                Country|Currency|Amount|Code|Rate
                                Invalid|Data|Format
                                Another|Invalid|Line";

        // Act
        var result = _parser.Parse(noValidRatesData);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Metadata["ErrorCode"].Should().Be(CnbErrorCode.NoValidRates);
    }

    [Fact]
    public void Parse_WithMixedValidAndInvalidRates_ReturnsOnlyValidRates()
    {
        // Arrange
        var mixedData = @"19 Aug 2025 #159
                        Country|Currency|Amount|Code|Rate
                        Australia|dollar|1|AUD|14.165
                        Invalid|Data|Format
                        Brazil|real|1|BRL|3.745
                        Another|Invalid|0|XXX|-5";

        // Act
        var result = _parser.Parse(mixedData);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Rates.Should().HaveCount(2);
        result.Value.Rates.Should().OnlyContain(r => r.Code == "AUD" || r.Code == "BRL");
    }

    [Fact]
    public void Parse_LogsDebugInformation()
    {
        // Arrange
        var validData = @"19 Aug 2025 #159
                        Country|Currency|Amount|Code|Rate
                        Australia|dollar|1|AUD|14.165";

        // Act
        _parser.Parse(validData);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Starting to parse CNB data")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}
