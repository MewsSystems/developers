using AutoMapper;

namespace Mews.ExchangeRate.API.Mapper;

public class DtoToDomainProfile : Profile
{
    public DtoToDomainProfile() 
    {
        CreateMap<Dtos.Currency, Domain.Currency>();
    }
}
