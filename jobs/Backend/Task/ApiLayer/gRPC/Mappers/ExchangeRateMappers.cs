using ApplicationLayer.DTOs.ExchangeRates;
using gRPC.Protos.Common;
using gRPC.Protos.ExchangeRates;
using Google.Protobuf.WellKnownTypes;

namespace gRPC.Mappers;

/// <summary>
/// Mappers for converting between ApplicationLayer DTOs and gRPC proto messages for exchange rates.
/// </summary>
public static class ExchangeRateMappers
{
    // ============================================================
    // GROUPED DATA FOR STREAMING
    // ============================================================

    /// <summary>
    /// Converts a list of LatestExchangeRateDto to grouped proto structure for streaming.
    /// Groups by Provider -> Base Currency -> Target Currency
    /// </summary>
    public static ExchangeRatesGroupedData ToProtoGroupedData(IEnumerable<LatestExchangeRateDto> rates)
    {
        var data = new ExchangeRatesGroupedData();

        // Group by provider
        var providerGroups = rates
            .GroupBy(r => new { r.ProviderId, r.ProviderCode, r.ProviderName })
            .OrderBy(g => g.Key.ProviderCode);

        foreach (var providerGroup in providerGroups)
        {
            var providerRatesGroup = new LatestProviderRatesGroup
            {
                ProviderId = providerGroup.Key.ProviderId,
                ProviderCode = providerGroup.Key.ProviderCode,
                ProviderName = providerGroup.Key.ProviderName
            };

            // Group by base currency within each provider
            var baseCurrencyGroups = providerGroup
                .GroupBy(r => r.BaseCurrencyCode)
                .OrderBy(g => g.Key);

            foreach (var baseCurrencyGroup in baseCurrencyGroups)
            {
                var baseCurrencyRates = new LatestBaseCurrencyGroup
                {
                    BaseCurrencyCode = baseCurrencyGroup.Key
                };

                // Add each target currency rate
                foreach (var rate in baseCurrencyGroup.OrderBy(r => r.TargetCurrencyCode))
                {
                    baseCurrencyRates.TargetCurrencies.Add(new LatestTargetCurrencyRate
                    {
                        TargetCurrencyCode = rate.TargetCurrencyCode,
                        Rate = rate.Rate.ToString("G29"), // Raw rate from provider
                        Multiplier = rate.Multiplier,
                        EffectiveRate = rate.EffectiveRate.ToString("G29"), // Preserve full precision
                        ValidDate = ToProtoDate(rate.ValidDate),
                        UpdatedAt = rate.Modified.HasValue
                            ? Timestamp.FromDateTimeOffset(rate.Modified.Value)
                            : Timestamp.FromDateTimeOffset(rate.Created)
                    });
                }

                providerRatesGroup.BaseCurrencies.Add(baseCurrencyRates);
            }

            data.Providers.Add(providerRatesGroup);
        }

        return data;
    }

    // ============================================================
    // CURRENT RATES (FLAT)
    // ============================================================

    public static CurrentExchangeRate ToProtoCurrentRate(CurrentExchangeRateDto dto)
    {
        return new CurrentExchangeRate
        {
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            SourceCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            Rate = dto.Rate.ToString("G29"), // Raw rate from provider
            Multiplier = dto.Multiplier,
            EffectiveRate = dto.EffectiveRate.ToString("G29"),
            ValidDate = ToProtoDate(dto.ValidDate)
        };
    }

    // ============================================================
    // LATEST RATES (FLAT)
    // ============================================================

    public static LatestExchangeRate ToProtoLatestRate(LatestExchangeRateDto dto)
    {
        return new LatestExchangeRate
        {
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            SourceCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            Rate = dto.Rate.ToString("G29"), // Raw rate from provider
            Multiplier = dto.Multiplier,
            EffectiveRate = dto.EffectiveRate.ToString("G29"),
            ValidDate = ToProtoDate(dto.ValidDate),
            UpdatedAt = dto.Modified.HasValue
                ? Timestamp.FromDateTimeOffset(dto.Modified.Value)
                : Timestamp.FromDateTimeOffset(dto.Created)
        };
    }

    // ============================================================
    // HISTORY (FLAT)
    // ============================================================

    public static ExchangeRateHistory ToProtoHistory(ExchangeRateHistoryDto dto)
    {
        return new ExchangeRateHistory
        {
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            SourceCurrencyCode = dto.BaseCurrencyCode,
            TargetCurrencyCode = dto.TargetCurrencyCode,
            Rate = dto.Rate.ToString("G29"), // Raw rate from provider
            Multiplier = dto.Multiplier,
            EffectiveRate = dto.EffectiveRate.ToString("G29"),
            ValidDate = ToProtoDate(dto.ValidDate)
        };
    }

    // ============================================================
    // CONVERSION
    // ============================================================

    public static ConversionResult ToProtoConversionResult(
        string sourceCurrencyCode,
        string targetCurrencyCode,
        decimal sourceAmount,
        decimal targetAmount,
        decimal effectiveRate,
        string validDate)
    {
        return new ConversionResult
        {
            SourceCurrencyCode = sourceCurrencyCode,
            TargetCurrencyCode = targetCurrencyCode,
            SourceAmount = sourceAmount.ToString("G29"),
            TargetAmount = targetAmount.ToString("G29"),
            EffectiveRate = effectiveRate.ToString("G29"),
            ValidDate = validDate
        };
    }

    // ============================================================
    // HELPER METHODS
    // ============================================================

    /// <summary>
    /// Converts DateOnly to proto Date message
    /// </summary>
    public static Date ToProtoDate(DateOnly date)
    {
        return new Date
        {
            Year = date.Year,
            Month = date.Month,
            Day = date.Day
        };
    }

    /// <summary>
    /// Converts proto Date message to DateOnly
    /// </summary>
    public static DateOnly FromProtoDate(Date date)
    {
        return new DateOnly(date.Year, date.Month, date.Day);
    }
}
