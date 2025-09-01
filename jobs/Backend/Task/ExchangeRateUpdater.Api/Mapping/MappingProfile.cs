using AutoMapper;
using ExchangeRateUpdater.Abstractions.Contracts;
using ExchangeRateUpdater.Api.Dtos;

namespace ExchangeRateUpdater.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ExchangeRate, ExchangeRateResponse>()
                .ForMember(d => d.SourceCurrency, m => m.MapFrom(s => s.SourceCurrency.Code))
                .ForMember(d => d.TargetCurrency, m => m.MapFrom(s => s.TargetCurrency.Code))
                .ForMember(d => d.Value, m => m.MapFrom(s => s.Value));
        }
    }
}
