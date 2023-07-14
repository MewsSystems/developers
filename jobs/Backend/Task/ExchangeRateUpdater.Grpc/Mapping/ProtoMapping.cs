using AutoMapper;
using ExchangeRateUpdater.Interface.DTOs;

namespace ExchangeRateUpdater.Grpc.Mapping
{
    public class ProtoMapping : Profile
    {
        public ProtoMapping()
        {
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<ExchangeRateDto, ExchangeRate>()
                .ForMember(d => d.StringValue, s => s.MapFrom(x => x.Value));

            CreateMap<decimal, DecimalValue>()
                .ForMember(d => d.Units, s => s.MapFrom(x => Math.Truncate(x)))
                .ForMember(d => d.Nanos, s => s.MapFrom(x => (x - Math.Truncate(x)) * 1000));
        }
    }
}
