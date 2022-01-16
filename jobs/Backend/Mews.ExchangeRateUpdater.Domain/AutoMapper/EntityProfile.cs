using AutoMapper;
using Mews.ExchangeRateUpdater.Domain.Enums;

namespace Mews.ExchangeRateUpdater.Domain.AutoMapper
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<CNB.ExchangeRate, Entities.ExchangeRate>()
                .ForMember(r => r.TargetCurrencyName, opt => opt.MapFrom(src => CurrencyCodes.CZK))
                .ForMember(r => r.SourceCurrencyName, opt => opt.MapFrom(src => src.CurrencyCode));
        }
    }
}