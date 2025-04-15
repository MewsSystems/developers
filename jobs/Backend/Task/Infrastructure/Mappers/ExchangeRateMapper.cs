using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.DTOs;

namespace ExchangeRateUpdater.Infrastructure.Mappers;

public interface IExchangeRateMapper
{
    IEnumerable<ExchangeRate> Map(RateDto rateDto);
}
public class ExchangeRateMapper : IExchangeRateMapper
{
    public IEnumerable<ExchangeRate> Map(RateDto rateDto)
    {
        if (rateDto == null)
            throw new ArgumentNullException(nameof(rateDto));

        var rates =  rateDto.ExchangeRateDtos.Select(dto =>
            new ExchangeRate(
                new Currency("CZK"),
                new Currency(dto.CurrencyCode),
                dto.Rate / dto.Amount
            ));

        return rates;
    }
}