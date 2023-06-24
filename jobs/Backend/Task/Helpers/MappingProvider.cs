using AutoMapper;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.DTOs;

namespace ToolsProvider.Helpers
{
    public static class MappingProvider
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Currency, CurrencyReadDTO>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper;
        }
    }
}
