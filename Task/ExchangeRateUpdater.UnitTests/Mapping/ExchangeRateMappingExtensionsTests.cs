using ExchangeRateUpdater.Application.Common.Mappings;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace UnitTests.Mapping;

using ExchangeRateUpdater.Application.ExchangeRates.Dtos;

public class ExchangeRateMappingExtensionsTests
{
    [Fact]
    public void ToDto_MapsCurrencyCodesAndValue()
    {
        var source = new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25.5m);

        var dto = source.ToDto();

        dto.SourceCurrencyCode.ShouldBe("USD");
        dto.TargetCurrencyCode.ShouldBe("CZK");
        dto.Value.ShouldBe(25.5m);
    }

    [Fact]
    public void ToExchangeRateEntity_CalculatesValue_WhenAmountNonZero()
    {
        var apiDto = new ExchangeRateApiDto
        {
            CurrencyCode = "USD",
            Amount = 100,
            Rate = 2500m
        };

        var entity = apiDto.ToExchangeRateEntity();

        entity.SourceCurrency.Code.ShouldBe("USD");
        entity.TargetCurrency.Code.ShouldBe("CZK");
        entity.Value.ShouldBe(2500m / 100m);
    }

    [Fact]
    public void ToExchangeRateEntity_HandlesZeroAmount_ReturnsZeroValue()
    {
        var apiDto = new ExchangeRateApiDto
        {
            CurrencyCode = "USD",
            Amount = 0,
            Rate = 1234m
        };

        var entity = apiDto.ToExchangeRateEntity();

        entity.Value.ShouldBe(0m);
    }
}
