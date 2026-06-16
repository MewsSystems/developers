using Domain.Entities;
using ExchangeRateUpdaterApi.Dtos.Response;

namespace ExchangeRateUpdaterApi.Mappers;

public static class ExchangeRateResponseMapper
{
    public static ExchangeRateResultDto ToExchangeRateResponseDto(this ExchangeRate exchangeRate)
    {
        return new ExchangeRateResultDto
        {
            SourceCurrency = exchangeRate.SourceCurrency.ToCurrencyDto(),
            TargetCurrency = exchangeRate.TargetCurrency.ToCurrencyDto(),
            Value = exchangeRate.Value
        };
    }
}