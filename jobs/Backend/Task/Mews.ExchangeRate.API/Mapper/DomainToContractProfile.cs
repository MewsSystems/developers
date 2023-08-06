using AutoMapper;

namespace Mews.ExchangeRate.API.Mapper;

public class DomainToContractProfile : Profile
{
    public DomainToContractProfile()
    {
        CreateMap<Domain.Currency, Dtos.Currency>();
        CreateMap<Domain.ExchangeRate, Dtos.ExchangeRate>();
    }
}
