using AutoMapper;
using MewsFinance.Application.UseCases.ExchangeRates.Queries;
using MewsFinance.Domain.Models;

namespace MewsFinance.Application.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile() 
        {
            CreateMap<ExchangeRate, ExchangeRateResponse>();
        }
    }
}
