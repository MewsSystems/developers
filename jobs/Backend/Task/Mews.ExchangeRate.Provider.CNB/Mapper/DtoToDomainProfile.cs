using AutoMapper;

namespace Mews.ExchangeRate.Provider.CNB.Mapper;

public class DtoToDomainProfile : Profile
{
    public DtoToDomainProfile()
    {
        CreateMap<Dtos.ExchangeRate, Domain.ExchangeRate>()
            .ConstructUsing(m =>
             new Domain.ExchangeRate(new Domain.Currency(m.CurrencyCode),
                    new Domain.Currency("CZK"),
                    m.Rate));
    }
}
