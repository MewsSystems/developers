using ApplicationLayer.DTOs.ExchangeRates;
using SOAP.Models.Common;
using SOAP.Models.ExchangeRates;

namespace SOAP.Converters;

/// <summary>
/// Converter extensions for exchange rate DTOs to SOAP models.
/// </summary>
public static class ExchangeRateSoapConverters
{
    /// <summary>
    /// Converts a collection of LatestExchangeRateDto to grouped SOAP response.
    /// Groups by Provider → Base Currency → Target Currencies.
    /// </summary>
    public static LatestExchangeRatesGroupedSoap[] ToNestedGroupedSoap(
        this IEnumerable<LatestExchangeRateDto> dtos)
    {
        return dtos
            .GroupBy(dto => new { dto.ProviderId, dto.ProviderCode, dto.ProviderName })
            .Select(providerGroup => new LatestExchangeRatesGroupedSoap
            {
                Provider = new ProviderInfoSoap
                {
                    Id = providerGroup.Key.ProviderId,
                    Code = providerGroup.Key.ProviderCode,
                    Name = providerGroup.Key.ProviderName
                },
                BaseCurrencies = providerGroup
                    .GroupBy(dto => dto.BaseCurrencyCode)
                    .Select(baseCurrencyGroup => new BaseCurrencyGroupSoap
                    {
                        BaseCurrency = baseCurrencyGroup.Key,
                        Rates = baseCurrencyGroup.Select(dto => new TargetCurrencyRateSoap
                        {
                            Id = dto.Id,
                            TargetCurrency = dto.TargetCurrencyCode,
                            RateInfo = new RateInfoSoap
                            {
                                Rate = dto.Rate,
                                Multiplier = dto.Multiplier,
                                EffectiveRate = dto.EffectiveRate
                            },
                            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                            FetchedAt = dto.Created.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            UpdatedAt = dto.Modified?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? dto.Created.ToString("yyyy-MM-ddTHH:mm:ssZ")
                        }).ToArray(),
                        TotalTargetCurrencies = baseCurrencyGroup.Count()
                    }).ToArray(),
                TotalBaseCurrencies = providerGroup.GroupBy(dto => dto.BaseCurrencyCode).Count(),
                TotalRates = providerGroup.Count()
            })
            .ToArray();
    }
}
