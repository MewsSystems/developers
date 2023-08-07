using AutoMapper;

namespace Mews.ExchangeRate.API.Mapper;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Domain.Currency, Dtos.Currency>();
        CreateMap<Domain.ExchangeRate, Dtos.ExchangeRate>();
    }
}
