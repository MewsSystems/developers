using AutoMapper;

namespace Mews.ExchangeRate.API.Mappers;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Domain.Currency, Dtos.Currency>();
        CreateMap<Domain.ExchangeRate, Dtos.ExchangeRate>();
    }
}
