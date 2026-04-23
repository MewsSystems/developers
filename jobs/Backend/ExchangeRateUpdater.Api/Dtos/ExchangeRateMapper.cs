using ExchangeRateUpdater.Api.Dtos;
using ExchangeRateUpdater.Core.Entities;

namespace ExchangeRateUpdater.Api.Dtos
{
    /// <summary>
    /// Maps domain exchange rate entities to DTOs.
    /// </summary>
    public static class ExchangeRateMapper
    {
        public static ExchangeRateDto ToDto(ExchangeRate rate)
        {
            return new ExchangeRateDto
            {
                SourceCurrency = rate.SourceCurrency.Code,
                TargetCurrency = rate.TargetCurrency.Code,
                Rate = rate.Rate,
                EffectiveDate = rate.EffectiveDate
            };
        }
    }
}
