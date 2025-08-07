using AutoMapper;

namespace Mews.ExchangeRate.API.Mappers;

public class DtoToDomainProfile : Profile
{
    public DtoToDomainProfile() 
    {
        CreateMap<Dtos.Currency, Domain.Currency>();
    }
}
