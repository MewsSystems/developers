using AutoMapper;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Infrastructure.Dtos;

namespace ExchangeRateUpdater.Application.MappingProfiles;

public class ExchangeRateProfile : Profile
{
    public ExchangeRateProfile()
    {
        CreateMap<CnbExchangeRateResponseItem, ExchangeRate>().ConstructUsing(exchangeRate =>
            new ExchangeRate(
                new Currency("CZK"),
                new Currency(exchangeRate.CurrencyCode),
                decimal.Divide(exchangeRate.Rate, exchangeRate.Amount)
            ));
    }
}
