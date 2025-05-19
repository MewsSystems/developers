using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Validators;
using FluentAssertions;
using Xunit;

namespace ExchangeRateUpdater.Tests.Unit.Parsers;

public class CurrencyPairParsingTests
{
    [Theory]
    [InlineData("USD/CZK", "USD", "CZK")]
    [InlineData("EUR/USD", "EUR", "USD")]
    [InlineData("GBP/JPY", "GBP", "JPY")]
    public void ParseCurrencyPair_ValidPairString_ReturnsCurrencyPair(string pairString, string expectedSource, string expectedTarget)
    {
        // Arrange
        BatchRateRequestValidator.BeIso4217Pair(pairString).Should().BeTrue();
        
        // Act
        var pair = new CurrencyPair 
        {
            SourceCurrency = pairString.Split('/')[0],
            TargetCurrency = pairString.Split('/')[1]
        };

        // Assert
        pair.Should().NotBeNull();
        pair.SourceCurrency.Should().Be(expectedSource);
        pair.TargetCurrency.Should().Be(expectedTarget);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("USD")]
    [InlineData("USD/")]
    [InlineData("/CZK")]
    [InlineData("USD-CZK")]
    [InlineData("USD/CZK/EUR")]
    public void ParseCurrencyPair_InvalidPairString_DetectedAsInvalid(string? invalidPairString)
    {
        // Act & Assert
        BatchRateRequestValidator.BeIso4217Pair(invalidPairString).Should().BeFalse();
    }

    [Fact]
    public void ValidateCurrencyPair_DoesNotAllowSpacesAroundSeparator()
    {
        // Arrange
        var pairWithSpaces = "USD / CZK";

        // Act & Assert
        BatchRateRequestValidator.BeIso4217Pair(pairWithSpaces).Should().BeFalse();
    }

    [Fact]
    public void ValidateCurrencyPair_DoesNotAllowLowercase()
    {
        // Arrange
        var lowercasePair = "usd/czk";

        // Act & Assert
        BatchRateRequestValidator.BeIso4217Pair(lowercasePair).Should().BeFalse();
    }
    
    [Theory]
    [InlineData("USD", true)]
    [InlineData("EUR", true)]
    [InlineData("CZK", true)]
    [InlineData("usd", false)] // Not uppercase
    [InlineData("US", false)] // Too short
    [InlineData("USDD", false)] // Too long
    [InlineData("US1", false)] // Contains digit
    [InlineData("US#", false)] // Contains special character
    [InlineData("", false)] // Empty
    [InlineData(null, false)] // Null
    public void IsIso4217CurrencyCode_ValidatesCorrectly(string? codeString, bool expectedResult)
    {
        // Act
        var result = BatchRateRequestValidator.IsIso4217CurrencyCode(codeString);

        // Assert
        result.Should().Be(expectedResult);
    }
    
    [Theory]
    [InlineData("USD/CZK", true)]
    [InlineData("EUR/USD", true)]
    [InlineData("usd/czk", false)] // Not uppercase
    [InlineData("USD-CZK", false)] // Wrong separator
    [InlineData("USDC/ZK", false)] // Wrong format (first code)
    [InlineData("USD/CZKK", false)] // Wrong format (second code)
    [InlineData("USD", false)] // Missing second code
    [InlineData("USD/", false)] // Missing second code
    [InlineData("/CZK", false)] // Missing first code
    [InlineData("", false)] // Empty string
    [InlineData("USD/CZK/EUR", false)] // Too many codes
    [InlineData("US1/CZK", false)] // Contains digit
    [InlineData("USD/CZ#", false)] // Contains special character
    public void IsIso4217Pair_ValidatesCorrectly(string pairString, bool expectedResult)
    {
        // Act
        var result = BatchRateRequestValidator.BeIso4217Pair(pairString);

        // Assert
        result.Should().Be(expectedResult);
    }
} 