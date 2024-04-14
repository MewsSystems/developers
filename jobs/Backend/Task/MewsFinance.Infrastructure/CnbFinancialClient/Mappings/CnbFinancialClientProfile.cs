using AutoMapper;
using MewsFinance.Domain.Models;

namespace MewsFinance.Infrastructure.CnbFinancialClient.Mappings
{
    public class CnbFinancialClientProfile : Profile
    {
        public CnbFinancialClientProfile()
        {
            CreateMap<CnbExchangeRate, ExchangeRate>()
                .ConstructUsing((rate, context) => new ExchangeRate(
                    sourceCurrency: new Currency(rate.CurrencyCode ?? string.Empty),
                    targetCurrency: new Currency((string)context.Items[MappingConstants.TargetCurrencyCode]),
                    rate.Rate));
        }       
    }
}
