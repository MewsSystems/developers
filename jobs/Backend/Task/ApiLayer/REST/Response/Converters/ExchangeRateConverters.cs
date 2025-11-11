using ApplicationLayer.DTOs.ExchangeRates;
using REST.Response.Models.Areas.ExchangeRates;
using REST.Response.Models.Common;

namespace REST.Response.Converters;

/// <summary>
/// Converters for transforming ExchangeRate DTOs to API response models.
/// </summary>
public static class ExchangeRateConverters
{
    /// <summary>
    /// Converts CurrentExchangeRateDto to CurrentExchangeRateResponse.
    /// </summary>
    public static CurrentExchangeRateResponse ToResponse(this CurrentExchangeRateDto dto)
    {
        return new CurrentExchangeRateResponse
        {
            CurrencyPair = new CurrencyPair
            {
                Base = dto.BaseCurrencyCode,
                Target = dto.TargetCurrencyCode
            },
            RateInfo = new RateInfo
            {
                Rate = dto.Rate,
                Multiplier = dto.Multiplier,
                EffectiveRate = dto.EffectiveRate
            },
            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
            Provider = new ProviderInfo
            {
                Id = 0, // CurrentExchangeRateDto doesn't have ProviderId
                Code = dto.ProviderCode,
                Name = string.Empty // CurrentExchangeRateDto doesn't have ProviderName
            },
            LastUpdated = dto.LastUpdated,
            DaysOld = dto.DaysOld
        };
    }

    /// <summary>
    /// Converts ExchangeRateHistoryDto to ExchangeRateHistoryResponse.
    /// </summary>
    public static ExchangeRateHistoryResponse ToResponse(this ExchangeRateHistoryDto dto)
    {
        return new ExchangeRateHistoryResponse
        {
            Provider = new ProviderInfo
            {
                Id = dto.ProviderId,
                Code = dto.ProviderCode,
                Name = dto.ProviderName
            },
            SourceCurrency = dto.SourceCurrencyCode,
            CurrencyPair = new CurrencyPair
            {
                Base = dto.BaseCurrencyCode,
                Target = dto.TargetCurrencyCode
            },
            RateInfo = new RateInfo
            {
                Rate = dto.Rate,
                Multiplier = dto.Multiplier,
                EffectiveRate = dto.EffectiveRate
            },
            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
            ChangeFromPrevious = dto.ChangeFromPrevious,
            ChangePercentage = dto.ChangePercentage
        };
    }

    /// <summary>
    /// Converts a currency conversion calculation to ConvertCurrencyResponse.
    /// </summary>
    public static ConvertCurrencyResponse ToConversionResponse(
        string fromCurrency,
        string toCurrency,
        decimal amount,
        decimal convertedAmount,
        decimal exchangeRate,
        string validDate)
    {
        return new ConvertCurrencyResponse
        {
            SourceCurrencyCode = fromCurrency,
            TargetCurrencyCode = toCurrency,
            SourceAmount = amount,
            TargetAmount = convertedAmount,
            EffectiveRate = exchangeRate,
            ValidDate = validDate
        };
    }

    /// <summary>
    /// Converts LatestExchangeRateDto to LatestExchangeRateResponse.
    /// </summary>
    public static LatestExchangeRateResponse ToResponse(this LatestExchangeRateDto dto)
    {
        return new LatestExchangeRateResponse
        {
            Id = dto.Id,
            Provider = new ProviderInfo
            {
                Id = dto.ProviderId,
                Code = dto.ProviderCode,
                Name = dto.ProviderName
            },
            CurrencyPair = new CurrencyPair
            {
                Base = dto.BaseCurrencyCode,
                Target = dto.TargetCurrencyCode
            },
            RateInfo = new RateInfo
            {
                Rate = dto.Rate,
                Multiplier = dto.Multiplier,
                EffectiveRate = dto.EffectiveRate
            },
            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
            Created = dto.Created,
            Modified = dto.Modified,
            DaysOld = dto.DaysOld,
            FreshnessStatus = dto.FreshnessStatus,
            MinutesSinceUpdate = dto.MinutesSinceUpdate,
            UpdateFreshness = dto.UpdateFreshness
        };
    }

    /// <summary>
    /// Converts ExchangeRateDto to ExchangeRateResponse.
    /// </summary>
    public static ExchangeRateResponse ToResponse(this ExchangeRateDto dto)
    {
        return new ExchangeRateResponse
        {
            Id = dto.Id,
            Provider = new ProviderInfo
            {
                Id = dto.ProviderId,
                Code = dto.ProviderCode,
                Name = dto.ProviderName
            },
            CurrencyPair = new CurrencyPair
            {
                Base = dto.BaseCurrencyCode,
                Target = dto.TargetCurrencyCode
            },
            RateInfo = new RateInfo
            {
                Rate = dto.Rate,
                Multiplier = dto.Multiplier,
                EffectiveRate = dto.EffectiveRate
            },
            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
            Created = dto.Created,
            Modified = dto.Modified
        };
    }

    /// <summary>
    /// Converts FetchResultDto to FetchResultResponse.
    /// </summary>
    public static FetchResultResponse ToResponse(this FetchResultDto dto)
    {
        return new FetchResultResponse
        {
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            Status = dto.Status,
            RatesImported = dto.RatesImported,
            RatesUpdated = dto.RatesUpdated,
            TotalRatesProcessed = dto.TotalRatesProcessed,
            ErrorMessage = dto.ErrorMessage,
            CompletedAt = dto.CompletedAt,
            DurationMs = (int)dto.Duration.TotalMilliseconds
        };
    }

    // ==================== GROUPED CONVERTERS (using GroupBy) ====================

    /// <summary>
    /// Converts a collection of CurrentExchangeRateDtos to grouped response by provider.
    /// Uses GroupBy to avoid duplicating provider information.
    /// </summary>
    public static IEnumerable<CurrentExchangeRatesByProviderResponse> ToGroupedResponse(
        this IEnumerable<CurrentExchangeRateDto> dtos)
    {
        return dtos
            .GroupBy(dto => new { dto.ProviderCode })
            .Select(group => new CurrentExchangeRatesByProviderResponse
            {
                Provider = new ProviderInfo
                {
                    Id = 0, // CurrentExchangeRateDto doesn't have ProviderId
                    Code = group.Key.ProviderCode,
                    Name = string.Empty // CurrentExchangeRateDto doesn't have ProviderName
                },
                Rates = group.Select(dto => new CurrentRateItem
                {
                    CurrencyPair = new CurrencyPair
                    {
                        Base = dto.BaseCurrencyCode,
                        Target = dto.TargetCurrencyCode
                    },
                    RateInfo = new RateInfo
                    {
                        Rate = dto.Rate,
                        Multiplier = dto.Multiplier,
                        EffectiveRate = dto.EffectiveRate
                    },
                    ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                    LastUpdated = dto.LastUpdated,
                    DaysOld = dto.DaysOld
                }).ToList()
            });
    }

    /// <summary>
    /// Converts a collection of LatestExchangeRateDtos to grouped response by provider.
    /// Uses GroupBy to avoid duplicating provider information.
    /// </summary>
    public static IEnumerable<LatestExchangeRatesByProviderResponse> ToGroupedResponse(
        this IEnumerable<LatestExchangeRateDto> dtos)
    {
        return dtos
            .GroupBy(dto => new { dto.ProviderId, dto.ProviderCode, dto.ProviderName })
            .Select(group => new LatestExchangeRatesByProviderResponse
            {
                Provider = new ProviderInfo
                {
                    Id = group.Key.ProviderId,
                    Code = group.Key.ProviderCode,
                    Name = group.Key.ProviderName
                },
                Rates = group.Select(dto => new LatestRateItem
                {
                    Id = dto.Id,
                    CurrencyPair = new CurrencyPair
                    {
                        Base = dto.BaseCurrencyCode,
                        Target = dto.TargetCurrencyCode
                    },
                    RateInfo = new RateInfo
                    {
                        Rate = dto.Rate,
                        Multiplier = dto.Multiplier,
                        EffectiveRate = dto.EffectiveRate
                    },
                    ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                    Created = dto.Created,
                    Modified = dto.Modified,
                    DaysOld = dto.DaysOld,
                    FreshnessStatus = dto.FreshnessStatus,
                    MinutesSinceUpdate = dto.MinutesSinceUpdate,
                    UpdateFreshness = dto.UpdateFreshness
                }).ToList()
            });
    }

    /// <summary>
    /// Converts a collection of ExchangeRateDtos to grouped response by provider.
    /// Uses GroupBy to avoid duplicating provider information.
    /// </summary>
    public static IEnumerable<ExchangeRatesByProviderResponse> ToGroupedResponse(
        this IEnumerable<ExchangeRateDto> dtos)
    {
        return dtos
            .GroupBy(dto => new { dto.ProviderId, dto.ProviderCode, dto.ProviderName })
            .Select(group => new ExchangeRatesByProviderResponse
            {
                Provider = new ProviderInfo
                {
                    Id = group.Key.ProviderId,
                    Code = group.Key.ProviderCode,
                    Name = group.Key.ProviderName
                },
                Rates = group.Select(dto => new ExchangeRateItem
                {
                    Id = dto.Id,
                    CurrencyPair = new CurrencyPair
                    {
                        Base = dto.BaseCurrencyCode,
                        Target = dto.TargetCurrencyCode
                    },
                    RateInfo = new RateInfo
                    {
                        Rate = dto.Rate,
                        Multiplier = dto.Multiplier,
                        EffectiveRate = dto.EffectiveRate
                    },
                    ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                    Created = dto.Created,
                    Modified = dto.Modified
                }).ToList()
            });
    }

    /// <summary>
    /// Converts a collection of ExchangeRateHistoryDtos to grouped response by provider.
    /// Uses GroupBy to avoid duplicating provider information.
    /// </summary>
    public static IEnumerable<ExchangeRateHistoryByProviderResponse> ToGroupedResponse(
        this IEnumerable<ExchangeRateHistoryDto> dtos)
    {
        return dtos
            .GroupBy(dto => new { dto.ProviderId, dto.ProviderCode, dto.ProviderName, dto.SourceCurrencyCode })
            .Select(group => new ExchangeRateHistoryByProviderResponse
            {
                Provider = new ProviderInfo
                {
                    Id = group.Key.ProviderId,
                    Code = group.Key.ProviderCode,
                    Name = group.Key.ProviderName
                },
                SourceCurrency = group.Key.SourceCurrencyCode,
                Rates = group.Select(dto => new HistoricalRateItem
                {
                    CurrencyPair = new CurrencyPair
                    {
                        Base = dto.BaseCurrencyCode,
                        Target = dto.TargetCurrencyCode
                    },
                    RateInfo = new RateInfo
                    {
                        Rate = dto.Rate,
                        Multiplier = dto.Multiplier,
                        EffectiveRate = dto.EffectiveRate
                    },
                    ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                    ChangeFromPrevious = dto.ChangeFromPrevious,
                    ChangePercentage = dto.ChangePercentage
                }).ToList()
            });
    }

    // ==================== GROUPED BY BASE CURRENCY ====================

    /// <summary>
    /// Converts a collection of CurrentExchangeRateDtos to grouped response by base currency.
    /// Shows all target currencies available from each base currency.
    /// </summary>
    public static IEnumerable<CurrentExchangeRatesByBaseCurrencyResponse> ToGroupedByBaseCurrencyResponse(
        this IEnumerable<CurrentExchangeRateDto> dtos)
    {
        return dtos
            .GroupBy(dto => dto.BaseCurrencyCode)
            .Select(group => new CurrentExchangeRatesByBaseCurrencyResponse
            {
                BaseCurrency = group.Key,
                Rates = group.Select(dto => new CurrentRateByBaseCurrencyItem
                {
                    Provider = new ProviderInfo
                    {
                        Id = 0, // CurrentExchangeRateDto doesn't have ProviderId
                        Code = dto.ProviderCode,
                        Name = string.Empty // CurrentExchangeRateDto doesn't have ProviderName
                    },
                    TargetCurrency = dto.TargetCurrencyCode,
                    RateInfo = new RateInfo
                    {
                        Rate = dto.Rate,
                        Multiplier = dto.Multiplier,
                        EffectiveRate = dto.EffectiveRate
                    },
                    ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                    LastUpdated = dto.LastUpdated,
                    DaysOld = dto.DaysOld
                }).ToList()
            });
    }

    /// <summary>
    /// Converts a collection of LatestExchangeRateDtos to grouped response by base currency.
    /// Shows all target currencies available from each base currency.
    /// </summary>
    public static IEnumerable<LatestExchangeRatesByBaseCurrencyResponse> ToGroupedByBaseCurrencyResponse(
        this IEnumerable<LatestExchangeRateDto> dtos)
    {
        return dtos
            .GroupBy(dto => dto.BaseCurrencyCode)
            .Select(group => new LatestExchangeRatesByBaseCurrencyResponse
            {
                BaseCurrency = group.Key,
                Rates = group.Select(dto => new LatestRateByBaseCurrencyItem
                {
                    Id = dto.Id,
                    Provider = new ProviderInfo
                    {
                        Id = dto.ProviderId,
                        Code = dto.ProviderCode,
                        Name = dto.ProviderName
                    },
                    TargetCurrency = dto.TargetCurrencyCode,
                    RateInfo = new RateInfo
                    {
                        Rate = dto.Rate,
                        Multiplier = dto.Multiplier,
                        EffectiveRate = dto.EffectiveRate
                    },
                    ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                    Created = dto.Created,
                    Modified = dto.Modified,
                    DaysOld = dto.DaysOld,
                    FreshnessStatus = dto.FreshnessStatus,
                    MinutesSinceUpdate = dto.MinutesSinceUpdate,
                    UpdateFreshness = dto.UpdateFreshness
                }).ToList()
            });
    }

    // ==================== NESTED GROUPED (Provider → Base Currency) ====================

    /// <summary>
    /// Converts a collection of CurrentExchangeRateDtos to nested grouped response.
    /// Groups by Provider → Base Currency → Target Currencies.
    /// Eliminates all duplicate provider and base currency information.
    /// </summary>
    public static IEnumerable<CurrentExchangeRatesGroupedResponse> ToNestedGroupedResponse(
        this IEnumerable<CurrentExchangeRateDto> dtos)
    {
        return dtos
            .GroupBy(dto => dto.ProviderCode)
            .Select(providerGroup => new CurrentExchangeRatesGroupedResponse
            {
                Provider = new ProviderInfo
                {
                    Id = 0, // CurrentExchangeRateDto doesn't have ProviderId
                    Code = providerGroup.Key,
                    Name = string.Empty // CurrentExchangeRateDto doesn't have ProviderName
                },
                BaseCurrencies = providerGroup
                    .GroupBy(dto => dto.BaseCurrencyCode)
                    .Select(baseCurrencyGroup => new CurrentBaseCurrencyGroup
                    {
                        BaseCurrency = baseCurrencyGroup.Key,
                        Rates = baseCurrencyGroup.Select(dto => new CurrentTargetCurrencyRate
                        {
                            TargetCurrency = dto.TargetCurrencyCode,
                            RateInfo = new RateInfo
                            {
                                Rate = dto.Rate,
                                Multiplier = dto.Multiplier,
                                EffectiveRate = dto.EffectiveRate
                            },
                            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                            LastUpdated = dto.LastUpdated,
                            DaysOld = dto.DaysOld
                        }).ToList()
                    }).ToList()
            });
    }

    /// <summary>
    /// Converts a collection of LatestExchangeRateDtos to nested grouped response.
    /// Groups by Provider → Base Currency → Target Currencies.
    /// Eliminates all duplicate provider and base currency information.
    /// </summary>
    public static IEnumerable<LatestExchangeRatesGroupedResponse> ToNestedGroupedResponse(
        this IEnumerable<LatestExchangeRateDto> dtos)
    {
        return dtos
            .GroupBy(dto => new { dto.ProviderId, dto.ProviderCode, dto.ProviderName })
            .Select(providerGroup => new LatestExchangeRatesGroupedResponse
            {
                Provider = new ProviderInfo
                {
                    Id = providerGroup.Key.ProviderId,
                    Code = providerGroup.Key.ProviderCode,
                    Name = providerGroup.Key.ProviderName
                },
                BaseCurrencies = providerGroup
                    .GroupBy(dto => dto.BaseCurrencyCode)
                    .Select(baseCurrencyGroup => new BaseCurrencyGroup
                    {
                        BaseCurrency = baseCurrencyGroup.Key,
                        Rates = baseCurrencyGroup.Select(dto => new TargetCurrencyRate
                        {
                            Id = dto.Id,
                            TargetCurrency = dto.TargetCurrencyCode,
                            RateInfo = new RateInfo
                            {
                                Rate = dto.Rate,
                                Multiplier = dto.Multiplier,
                                EffectiveRate = dto.EffectiveRate
                            },
                            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                            Created = dto.Created,
                            Modified = dto.Modified,
                            DaysOld = dto.DaysOld,
                            FreshnessStatus = dto.FreshnessStatus,
                            MinutesSinceUpdate = dto.MinutesSinceUpdate,
                            UpdateFreshness = dto.UpdateFreshness
                        }).ToList()
                    }).ToList()
            });
    }

    /// <summary>
    /// Converts a collection of ExchangeRateHistoryDtos to nested grouped response.
    /// Groups by Provider → Base Currency → Historical Rates.
    /// Eliminates all duplicate provider and base currency information.
    /// </summary>
    public static IEnumerable<ExchangeRateHistoryGroupedResponse> ToNestedGroupedResponse(
        this IEnumerable<ExchangeRateHistoryDto> dtos)
    {
        return dtos
            .GroupBy(dto => new { dto.ProviderId, dto.ProviderCode, dto.ProviderName, dto.SourceCurrencyCode })
            .Select(providerGroup => new ExchangeRateHistoryGroupedResponse
            {
                Provider = new ProviderInfo
                {
                    Id = providerGroup.Key.ProviderId,
                    Code = providerGroup.Key.ProviderCode,
                    Name = providerGroup.Key.ProviderName
                },
                SourceCurrency = providerGroup.Key.SourceCurrencyCode,
                BaseCurrencies = providerGroup
                    .GroupBy(dto => dto.BaseCurrencyCode)
                    .Select(baseCurrencyGroup => new HistoricalBaseCurrencyGroup
                    {
                        BaseCurrency = baseCurrencyGroup.Key,
                        Rates = baseCurrencyGroup.Select(dto => new HistoricalTargetCurrencyRate
                        {
                            TargetCurrency = dto.TargetCurrencyCode,
                            RateInfo = new RateInfo
                            {
                                Rate = dto.Rate,
                                Multiplier = dto.Multiplier,
                                EffectiveRate = dto.EffectiveRate
                            },
                            ValidDate = dto.ValidDate.ToString("yyyy-MM-dd"),
                            ChangeFromPrevious = dto.ChangeFromPrevious,
                            ChangePercentage = dto.ChangePercentage
                        }).ToList()
                    }).ToList()
            });
    }
}
