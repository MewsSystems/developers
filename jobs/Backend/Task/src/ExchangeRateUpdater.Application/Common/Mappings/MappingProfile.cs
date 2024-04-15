using AutoMapper;
using ExchangeRateUpdater.Application.ExchangeRates.Dto;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Application.Common.Mappings
{
    /// <summary>
    /// Represents a mapping profile for AutoMapper configuration,
    /// mapping domain objects to their corresponding DTOs.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<ExchangeRate, ExchangeRateDto>();
            CreateMap<Currency, CurrencyDto>();
        }
    }
}
