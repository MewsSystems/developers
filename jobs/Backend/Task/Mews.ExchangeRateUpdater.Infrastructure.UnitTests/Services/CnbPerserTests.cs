using Mews.ExchangeRateUpdater.Infrastructure.Dtos;
using Mews.ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Mews.ExchangeRateUpdater.Infrastructure.UnitTests.Services;

public class CnbParserTests
{
    private readonly CnbParser _parser;

    public CnbParserTests()
    {
        _parser = new CnbParser(Mock.Of<ILogger<CnbParser>>());
    }

    [Fact]
    public void ReturnsExchangeRates_WhenValidResponse()
    {
        // Arrange
        var response = new CnbResponse
        {
            Rates = new List<CnbRateDto>
            {
                new CnbRateDto { CurrencyCode = "USD", Amount = 1, Rate = 22.345m },
                new CnbRateDto { CurrencyCode = "EUR", Amount = 1, Rate = 24.123m }
            }
        };

        // Act
        var result = _parser.Parse(response).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.SourceCurrency.Code == "USD" && r.Value == 22.345m);
        Assert.Contains(result, r => r.SourceCurrency.Code == "EUR" && r.Value == 24.123m);
    }

    [Fact]
    public void SkipsInvalidRates_WhenAmountIsZero()
    {
        // Arrange
        var response = new CnbResponse
        {
            Rates = new List<CnbRateDto>
            {
                new CnbRateDto { CurrencyCode = "USD", Amount = 0, Rate = 22.345m },
                new CnbRateDto { CurrencyCode = "EUR", Amount = 1, Rate = 24.123m }
            }
        };

        // Act
        var result = _parser.Parse(response).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("EUR", result[0].SourceCurrency.Code);
    }

    [Fact]
    public void ReturnsEmpty_WhenNoRates()
    {
        // Arrange
        var response = new CnbResponse { Rates = [] };

        // Act
        var result = _parser.Parse(response);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ReturnsEmpty_WhenResponseIsNull()
    {
        var result = _parser.Parse(null!);
        Assert.Empty(result);
    }
}