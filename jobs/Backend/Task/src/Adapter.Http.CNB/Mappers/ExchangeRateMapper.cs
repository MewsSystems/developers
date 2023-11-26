using Adapter.Http.CNB.Dtos.Response;
using Domain.Entities;
using Domain.ValueTypes;

namespace Adapter.Http.CNB.Mappers;

public static class ExchangeRateMapper
{
    public static ExchangeRate ToExchangeRate(this ExchangeRateDto exchangeRateDto)
    {
        return new ExchangeRate(
            new Currency("CZK"), // CNB only supports CZK -> <other_currency> exchange rates
            new Currency(exchangeRateDto.TargetCurrency),
            exchangeRateDto.Rate);
    }
}