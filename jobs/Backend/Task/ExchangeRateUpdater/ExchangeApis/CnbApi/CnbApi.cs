using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExchangeApis.CnbApi.DTOs;

namespace ExchangeRateUpdater.ExchangeApis.CnbApi;

public class CnbApi(HttpClient client) : IExchangeApi
{
    private readonly HttpClient _client = client;

    public async Task<IEnumerable<ExchangeRate>> GetAllRates(CancellationToken cancellationToken = default)
    {
        var allRates = await Task.WhenAll(
            GetExrates(cancellationToken),
            GetFxrates(cancellationToken));

        return allRates.SelectMany(x => x);
    }

    private async Task<IEnumerable<ExchangeRate>> GetExrates(CancellationToken cancellationToken)
    {
        var exrates = await _client.GetFromJsonAsync<RatesResultDto>("exrates/daily?lang=EN", cancellationToken);
        return exrates.ToExchangeRates("CZK");
    }

    private async Task<IEnumerable<ExchangeRate>> GetFxrates(CancellationToken cancellationToken)
    {
        /* 
         * Data is published the last working day of each month and valid for the entire next month
         * so we always need to get the data for the previous month.
        */
        var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"); // Czechia's timezone
        var lastMonthDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone).AddMonths(-1);
        var yearMonth = lastMonthDateTime.ToString("yyyy-MM");

        var exrates = await _client.GetFromJsonAsync<RatesResultDto>($"fxrates/daily-month?lang=EN&yearMonth={yearMonth}", cancellationToken);
        return exrates.ToExchangeRates("CZK");
    }
}
