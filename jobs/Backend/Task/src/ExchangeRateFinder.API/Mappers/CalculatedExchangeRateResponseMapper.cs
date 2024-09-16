using ExchangeRateFinder.API.ViewModels;
using ExchangeRateFinder.Domain.Entities;

namespace ExchangeRateFinder.API.Mappers
{
    public interface ICalculatedExchangeRateResponseMapper
    {
        CalculatedExchangeRateResponseModel Map(CalculatedExchangeRate domainEntity);
    }

    public class CalculatedExchangeRateResponseMapper : ICalculatedExchangeRateResponseMapper
    {
        public CalculatedExchangeRateResponseModel Map(CalculatedExchangeRate domainEntity)
            => new CalculatedExchangeRateResponseModel() { 
                SourceCurrencyCode = domainEntity.SourceCurrencyCode,
                TargetCurrencyCode = domainEntity.TargetCurrencyCode,
                Rate = domainEntity.Rate
            };
    }
}
