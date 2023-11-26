using Domain.Entities;
using ExchangeRateUpdaterApi.Dtos.Request;

namespace ExchangeRateUpdaterApi.Mappers;

public static class ExchangeRateRequestMapper
{
    public static ExchangeRateRequest ToExchangeRateRequest(this ExchangeRateDetailsDto exchangeRateDetailsDto)
    {
        return new ExchangeRateRequest(
            exchangeRateDetailsDto.SourceCurrency.ToCurrency(), 
            exchangeRateDetailsDto.TargetCurrency.ToCurrency());
    }
}