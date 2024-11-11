using AutoMapper;
using CNB = Cnb.Api.Client;

namespace ExchangeRateUpdater.Mapping
{
    public class ExRatesDailyProfile : Profile
    {
        public ExRatesDailyProfile()
        {
            CreateMap<CNB.ExRateDailyRest, ExchangeRate>(MemberList.Destination)
            .ForCtorParam("sourceCurrency", opt => opt.MapFrom(src => new Currency(src.CurrencyCode)))
            .ForCtorParam("targetCurrency", opt => opt.MapFrom(src => new Currency("CZK")))
            .ForCtorParam("value", opt => opt.MapFrom(src => (decimal)(src.Rate ?? 0) / (src.Amount ?? 1)));
            ;
        }
    }
}