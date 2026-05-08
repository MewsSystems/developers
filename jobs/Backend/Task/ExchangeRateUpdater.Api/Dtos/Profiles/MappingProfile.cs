using AutoMapper;
using ExchangeRateUpdater.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.API.Dtos.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CurrencyRequestDto, Currency>();

            CreateMap<ExchangeRate, ExchangeRateResponseDto>()
                .ForMember(dest => dest.SourceCurrencyCode, opt => opt.MapFrom(src => src.SourceCurrency.Code))
                .ForMember(dest => dest.TargetCurrencyCode, opt => opt.MapFrom(src => src.TargetCurrency.Code))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Value));
        }
    }
}
