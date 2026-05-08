using AutoMapper;
using ExchangeRate.Api.Controllers.Models;
using ExchangeRate.Api.Models;
using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Api.Mappers
{
    public class MappingModelProfile : Profile
    {
        public MappingModelProfile()
        {
            CreateMap<CurrencyModel, CurrencyDTO>().ReverseMap();
            CreateMap<CurrenciesModel, CurrenciesDTO>().ReverseMap();

            CreateMap<ExchangeRateProviderDTO, ExchangeRateProviderModel>().ReverseMap();
            CreateMap<ExchangeRateProviderResultDTO, ExchangeRatesResultModel>().ReverseMap();
            CreateMap<ExchangeRatesDTO, ExchangeRateModel>().ReverseMap(); 

        }
    }
}
