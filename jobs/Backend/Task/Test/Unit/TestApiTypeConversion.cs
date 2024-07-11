using FluentAssertions;
using ExchangeRateUpdater.ExchangeApis.CnbApi.DTOs;

namespace Test.Unit;

public class TestApiTypeConversion
{
    [Fact]
    public void WhenRateDtoToExchangeRate_CorrectValue()
    {
        // Given
        const string sourceCurrencyCode = "LKR";
        const string targetCurrencyCode = "CZK";
        const decimal rateValue = 7.645m;
        const int exchangedAmount = 100;
        var cnbRateDto = new RateDto
        {
            Currency = "rupee",
            Rate = rateValue,
            Amount = exchangedAmount,
            CurrencyCode = sourceCurrencyCode,
            Country = "Sri Lanka",
            Order = 6,
            ValidFor = "2024-06-28"
        };

        // When
        var rate = cnbRateDto.ToExchangeRate(targetCurrencyCode);

        // Then
        rate.SourceCurrency.Code.Should().Be(sourceCurrencyCode);
        rate.TargetCurrency.Code.Should().Be(targetCurrencyCode);
        rate.Value.Should().Be(rateValue / exchangedAmount);
    }
}