using ExchangeRateUpdater.Abstractions.Model;
using ExchangeRateUpdater.CnbClient.Dtos;

namespace ExchangeRateUpdater.CnbClient.Extensions;

/// <summary>
/// Provides extension methods for mapping ExchangeRateDto to CurrencyValue.
/// </summary>
public static class ExchangeRateCnbMapper
{
    /// <summary>
    /// Maps an ExchangeRateDto to a CurrencyValue.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static CurrencyValue MapToCurrencyValue(this ExchangeRateDto dto)
    {
        return new CurrencyValue
        {
            ValidFor = dto.ValidFor,
            Amount = dto.Amount,
            CurrencyCode = dto.CurrencyCode,
            Rate = dto.Rate
        };
    }
}