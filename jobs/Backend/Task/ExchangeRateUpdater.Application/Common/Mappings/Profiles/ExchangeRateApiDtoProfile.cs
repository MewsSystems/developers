namespace ExchangeRateUpdater.Application.Common.Mappings.Profiles;

using AutoMapper;
using ExchangeRateUpdater.Application.ExchangeRates.Dtos;
using ExchangeRateUpdater.Domain.Entities;

public class ExchangeRateApiDtoProfile : Profile
{
    public ExchangeRateApiDtoProfile()
    {
        CreateMap<ExchangeRateApiDto, ExchangeRate>().ConstructUsing(x =>
            new ExchangeRate(new Currency(x.CurrencyCode), new Currency("CZK"), decimal.Divide(x.Rate, x.Amount)));
        
    }
}