using Mews.ExchangeRateUpdater.Dtos;
using Mews.ExchangeRateUpdater.Services.Models;

namespace Mews.ExchangeRateUpdater.Services.DtoMappers
{
    /// <summary>
    /// This is the mapper which is used to map CurrencyModel to CurrencyDto
    /// </summary>
    public static class CurrencyDtoMapper
    {
        public static CurrencyDto? ToCurrencyDto(this CurrencyModel currencyModel)
        {
            if (currencyModel == null) return null;

            return new CurrencyDto(currencyModel.Code);
        }
    }
}
