namespace UnitTests.Fakers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using ExchangeRateUpdater.Application.ExchangeRates.Dtos;
using ExchangeRateUpdater.Application.Common.Interfaces;
using ExchangeRateUpdater.Domain.Enums;

public class FakeExchangeRateApiClient : IExchangeRateApiClient
{
    public Func<DateTime?, Language?, CancellationToken, Task<IEnumerable<ExchangeRateApiDto>>>? OnGetExchangeRatesAsync { get; set; }
    public Func<int, Task<List<ExchangeRateApiDto>>>? OnGetDefaultExchangeRatesForYearAsync { get; set; }

    public Task<IEnumerable<ExchangeRateApiDto>> GetExchangeRatesAsync(DateTime? date, Language? language)
    {
        if (OnGetExchangeRatesAsync != null)
            return OnGetExchangeRatesAsync(date, language ?? Language.EN, CancellationToken.None);

        return Task.FromResult(Enumerable.Empty<ExchangeRateApiDto>());
    }

    public Task<IEnumerable<ExchangeRateApiDto>> GetExchangeRatesAsync(DateTime? date, Language? language, CancellationToken ct)
    {
        if (OnGetExchangeRatesAsync != null)
            return OnGetExchangeRatesAsync(date, language ?? Language.EN, ct);

        return Task.FromResult(Enumerable.Empty<ExchangeRateApiDto>());
    }

    public Task<List<ExchangeRateApiDto>> GetDefaultExchangeRatesForYearAsync(int year)
    {
        if (OnGetDefaultExchangeRatesForYearAsync != null)
            return OnGetDefaultExchangeRatesForYearAsync(year);

        return Task.FromResult(new List<ExchangeRateApiDto>());
    }
}
