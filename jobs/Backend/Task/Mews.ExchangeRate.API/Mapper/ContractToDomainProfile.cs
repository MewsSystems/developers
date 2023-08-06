using AutoMapper;

namespace Mews.ExchangeRate.API.Mapper;

public class ContractToDomainProfile : Profile
{
    public ContractToDomainProfile() 
    {
        CreateMap<Dtos.Currency, Domain.Currency>();
    }
}
