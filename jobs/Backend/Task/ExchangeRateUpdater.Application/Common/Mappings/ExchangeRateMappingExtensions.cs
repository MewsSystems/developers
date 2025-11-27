namespace ExchangeRateUpdater.Application.Common.Mappings;

using ExchangeRates.Dtos;
using Domain.Entities;
using Domain.ValueObjects;

public static class ExchangeRateMappingExtensions
{
    private const string DefaultTargetCurrencyCode = "CZK";
    
    public static ExchangeRateDto ToDto(this ExchangeRate source)
    {
        return new ExchangeRateDto
        {
            SourceCurrencyCode = source.SourceCurrency.Code,
            TargetCurrencyCode = "CZK",
            Value = source.Value
        };
    }

    public static ExchangeRate ToExchangeRateEntity(this ExchangeRateApiDto apiDto)
    {
        var sourceCurrency = new Currency(apiDto.CurrencyCode);
        var targetCurrency = new Currency(DefaultTargetCurrencyCode);
        var value = apiDto.Amount == 0 ? 0m : decimal.Divide(apiDto.Rate, apiDto.Amount);

        return new ExchangeRate(sourceCurrency, targetCurrency, value);
    }

    public static ExchangeRate ToEntity(this ExchangeRateDto dto)
    {
        var source = new Domain.ValueObjects.Currency(dto.SourceCurrencyCode);
        var target = new Domain.ValueObjects.Currency(dto.TargetCurrencyCode);
        return new ExchangeRate(source, target, dto.Value);
    }
}