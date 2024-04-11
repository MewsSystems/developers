using AutoMapper;
using MewsFinance.Domain.Models;

namespace MewsFinance.Infrastructure.CnbFinancialClient.Mappings
{
    public class CnbFinancialClientProfile : Profile
    {
        public CnbFinancialClientProfile()
        {
            CreateMap<CnbExchangeRate, ExchangeRate>()
                .ConstructUsing(rate => new ExchangeRate(
                    new Currency(rate.CurrencyCode),
                    new Currency("CZK"),
                    rate.Rate));
        }       
    }
}
