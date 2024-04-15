using AutoMapper;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi.Dto;

namespace ExchangeRateUpdater.Infrastructure.Mappings
{
    /// <summary>
    /// Represents a mapping profile for AutoMapper configuration,
    /// mapping CzechNationalBankApi DTOs to their corresponding domain objects.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<ExDateDailyRest, ExchangeRate>().ConstructUsing(x => new ExchangeRate(new Currency(x.CurrencyCode), new Currency("CZK"), decimal.Divide(x.Rate, x.Amount))); ;
        }
    }
}
