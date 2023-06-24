using AutoMapper;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.DTOs;

namespace ExchangeRateUpdater.Profiles
{
     class ProfilesMapping : Profile
    {
        public ProfilesMapping()
        {
            CreateMap<Currency, CurrencyReadDTO>();
        }
    }
}
