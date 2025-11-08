using ApplicationLayer.DTOs.ExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.ConvertCurrency;
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

    /// <summary>
    /// Converts a collection of CurrentExchangeRateDto to grouped SOAP response.
    /// Groups by Provider → Base Currency → Target Currencies.
    /// </summary>
    public static CurrentExchangeRatesGroupedSoap[] ToCurrentNestedGroupedSoap(
        this IEnumerable<CurrentExchangeRateDto> dtos)
    {
        return dtos
            .GroupBy(dto => dto.ProviderCode)
            .Select(providerGroup => new CurrentExchangeRatesGroupedSoap
            {
                Provider = new ProviderInfoSoap
                {
                    Id = 0, // CurrentExchangeRateDto doesn't have ProviderId
                    Code = providerGroup.Key,
                    Name = providerGroup.Key
                },
                BaseCurrencies = providerGroup
                    .GroupBy(dto => dto.BaseCurrencyCode)
                    .Select(baseCurrencyGroup => new CurrentBaseCurrencyGroupSoap
                    {
                        BaseCurrency = baseCurrencyGroup.Key,
                        Rates = baseCurrencyGroup.Select(dto => new CurrentTargetCurrencyRateSoap
                        {
                            TargetCurrency = dto.TargetCurrencyCode,
                            RateInfo = new RateInfoSoap
                            {
                                Rate = dto.Rate,
                                Multiplier = dto.Multiplier,
                                EffectiveRate = dto.EffectiveRate
                            },
                            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                            LastUpdated = dto.LastUpdated.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            DaysOld = dto.DaysOld
                        }).ToArray(),
                        TotalTargetCurrencies = baseCurrencyGroup.Count()
                    }).ToArray(),
                TotalBaseCurrencies = providerGroup.GroupBy(dto => dto.BaseCurrencyCode).Count(),
                TotalRates = providerGroup.Count()
            })
            .ToArray();
    }

    /// <summary>
    /// Converts a CurrencyConversionResult to SOAP model.
    /// </summary>
    public static CurrencyConversionSoap ToSoap(this CurrencyConversionResult result)
    {
        return new CurrencyConversionSoap
        {
            SourceAmount = result.SourceAmount,
            TargetAmount = result.TargetAmount,
            SourceCurrency = result.SourceCurrencyCode,
            TargetCurrency = result.TargetCurrencyCode,
            RateInfo = new RateInfoSoap
            {
                Rate = result.Rate,
                Multiplier = result.Multiplier,
                EffectiveRate = result.EffectiveRate
            },
            ValidDate = result.ValidDate.ToString("yyyy-MM-dd"),
            Provider = new ProviderInfoSoap
            {
                Id = result.ProviderId,
                Code = string.Empty, // Not available in CurrencyConversionResult
                Name = result.ProviderName
            }
        };
    }

    /// <summary>
    /// Converts an ExchangeRateDto to SOAP model.
    /// </summary>
    public static ExchangeRateSoap ToSoap(this ExchangeRateDto dto)
    {
        return new ExchangeRateSoap
        {
            Id = dto.Id,
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            BaseCurrencyId = dto.BaseCurrencyId,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyId = dto.TargetCurrencyId,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            Multiplier = dto.Multiplier,
            Rate = dto.Rate,
            EffectiveRate = dto.EffectiveRate,
            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
            Created = dto.Created,
            Modified = dto.Modified
        };
    }

    /// <summary>
    /// Converts a collection of LatestExchangeRateDto to flat SOAP models.
    /// </summary>
    public static LatestExchangeRateSoap[] ToFlatSoap(this IEnumerable<LatestExchangeRateDto> dtos)
    {
        return dtos.Select(dto => new LatestExchangeRateSoap
        {
            Id = dto.Id,
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            BaseCurrencyId = dto.BaseCurrencyId,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyId = dto.TargetCurrencyId,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            Rate = dto.Rate,
            Multiplier = dto.Multiplier,
            EffectiveRate = dto.EffectiveRate,
            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
            Created = dto.Created,
            Modified = dto.Modified,
            DaysOld = dto.DaysOld,
            FreshnessStatus = dto.FreshnessStatus,
            MinutesSinceUpdate = dto.MinutesSinceUpdate,
            UpdateFreshness = dto.UpdateFreshness
        }).ToArray();
    }

    /// <summary>
    /// Converts a collection of CurrentExchangeRateDto to flat SOAP models.
    /// </summary>
    public static CurrentExchangeRateSoap[] ToFlatSoap(this IEnumerable<CurrentExchangeRateDto> dtos)
    {
        return dtos.Select(dto => new CurrentExchangeRateSoap
        {
            BaseCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            Rate = dto.Rate,
            Multiplier = dto.Multiplier,
            EffectiveRate = dto.EffectiveRate,
            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
            ProviderCode = dto.ProviderCode,
            LastUpdated = dto.LastUpdated,
            DaysOld = dto.DaysOld
        }).ToArray();
    }

    /// <summary>
    /// Converts a collection of ExchangeRateHistoryDto to flat SOAP models.
    /// </summary>
    public static ExchangeRateHistorySoap[] ToFlatSoap(this IEnumerable<ExchangeRateHistoryDto> dtos)
    {
        return dtos.Select(dto => new ExchangeRateHistorySoap
        {
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            SourceCurrencyCode = dto.SourceCurrencyCode,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
            Rate = dto.Rate,
            Multiplier = dto.Multiplier,
            EffectiveRate = dto.EffectiveRate,
            ChangeFromPrevious = dto.ChangeFromPrevious,
            ChangePercentage = dto.ChangePercentage
        }).ToArray();
    }

    /// <summary>
    /// Converts a collection of ExchangeRateHistoryDto to grouped SOAP models.
    /// Groups by Provider → Base Currency → Target Currencies with historical data.
    /// </summary>
    public static ExchangeRateHistoryGroupedSoap[] ToGroupedSoap(this IEnumerable<ExchangeRateHistoryDto> dtos)
    {
        return dtos
            .GroupBy(dto => new { dto.ProviderId, dto.ProviderCode, dto.ProviderName })
            .Select(providerGroup => new ExchangeRateHistoryGroupedSoap
            {
                ProviderId = providerGroup.Key.ProviderId,
                ProviderCode = providerGroup.Key.ProviderCode,
                ProviderName = providerGroup.Key.ProviderName,
                BaseCurrencies = providerGroup
                    .GroupBy(dto => dto.BaseCurrencyCode)
                    .Select(baseCurrencyGroup => new HistoryBaseCurrencyGroupSoap
                    {
                        BaseCurrencyCode = baseCurrencyGroup.Key,
                        TargetCurrencies = baseCurrencyGroup
                            .GroupBy(dto => dto.TargetCurrencyCode)
                            .Select(targetCurrencyGroup => new HistoryTargetCurrencyRateSoap
                            {
                                TargetCurrencyCode = targetCurrencyGroup.Key,
                                History = targetCurrencyGroup.Select(dto => new HistoryDataPointSoap
                                {
                                    ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                                    Rate = dto.Rate,
                                    Multiplier = dto.Multiplier,
                                    EffectiveRate = dto.EffectiveRate,
                                    ChangeFromPrevious = dto.ChangeFromPrevious,
                                    ChangePercentage = dto.ChangePercentage
                                }).ToArray()
                            }).ToArray()
                    }).ToArray()
            })
            .ToArray();
    }
}
