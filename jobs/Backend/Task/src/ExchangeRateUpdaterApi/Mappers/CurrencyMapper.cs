using Domain.ValueTypes;
using ExchangeRateUpdaterApi.Dtos;

namespace ExchangeRateUpdaterApi.Mappers;

public static class CurrencyMapper
{
    public static Currency ToCurrency(this CurrencyDto currencyDto)
    {
        return new Currency(currencyDto.Code);
    }

    public static CurrencyDto ToCurrencyDto(this Currency currency)
    {
        return new CurrencyDto
        {
            Code = currency.Code
        };
    }
}