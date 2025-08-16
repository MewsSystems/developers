using AutoMapper;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;

namespace ExchangeRateUpdater.Application.Profiles;

public class ExchangeRateProfile : Profile
{
    public ExchangeRateProfile()
    {
        CreateMap<CzechNationalBankExchangeRate, ExchangeRate>()
            .ConstructUsing(src => new ExchangeRate(new Currency(src.CurrencyCode), new Currency("CZK"), decimal.Divide(src.Rate, src.Amount)));
    }
}
