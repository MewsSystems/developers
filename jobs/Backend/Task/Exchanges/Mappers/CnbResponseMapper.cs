using ExchangeRateUpdater.Model.Cnb;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ExchangeRateUpdater.Exchanges.Mappers;

public static class CnbDailyRateResponseMapper
{
    public static IEnumerable<CnbRate> MapToExchangeRates(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var cnbRates = JsonSerializer.Deserialize<RateResponse>(json, options);

        if (cnbRates == null || cnbRates.Rates == null)
        {
            throw new InvalidOperationException("Failed to deserialize CNB data.");
        }

        return cnbRates.Rates;
    }
}
