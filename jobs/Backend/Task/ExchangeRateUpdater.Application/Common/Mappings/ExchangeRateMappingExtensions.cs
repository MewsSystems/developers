namespace ExchangeRateUpdater.Application.Common.Mappings;

using ExchangeRates.Dtos;
using Domain.Entities;
using Domain.ValueObjects;

public static class ExchangeRateMappingExtensions
{
    // Maps domain ExchangeRate -> DTO used by your application/API
    public static ExchangeRateDto ToDto(this ExchangeRate source)
    {
        return new ExchangeRateDto
        {
            SourceCurrencyCode = source.SourceCurrency.Code,
            TargetCurrencyCode = "CZK",
            Value = source.Value
        };
    }

    // Maps incoming API DTO -> domain ExchangeRate (mirrors ConstructUsing in your AutoMapper profile)
    public static ExchangeRate ToExchangeRateEntity(this ExchangeRateApiDto apiDto)
    {
        var sourceCurrency = new Currency(apiDto.CurrencyCode);
        var targetCurrency = new Currency("CZK");
        var value = apiDto.Amount == 0 ? 0m : decimal.Divide(apiDto.Rate, apiDto.Amount);

        return new ExchangeRate(sourceCurrency, targetCurrency, value);
    }
}