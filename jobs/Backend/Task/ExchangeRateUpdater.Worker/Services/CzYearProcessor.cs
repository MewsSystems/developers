namespace ExchangeRateUpdater.Worker.Services;

using System.Globalization;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Utils;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;

public interface ICzYearProcessor
{
    Task<DateTime?> ProcessYearAsync(int year, CancellationToken ct);
}

public class CzYearProcessor : ICzYearProcessor
{
    private readonly IExchangeRateApiClient _apiClient;
    private readonly ICacheRepository _cacheRepository;
    private readonly ILogger<CzYearProcessor> _logger;
    private readonly int _ttlYears;

    public CzYearProcessor(IExchangeRateApiClient apiClient, ICacheRepository cacheRepository,
        ILogger<CzYearProcessor> logger, int ttlYears = 4)
    {
        Ensure.Argument.NotNull(apiClient, nameof(apiClient));
        Ensure.Argument.NotNull(cacheRepository, nameof(cacheRepository));
        Ensure.Argument.NotNull(logger, nameof(logger));
        _apiClient = apiClient;
        _cacheRepository = cacheRepository;
        _logger = logger;
        _ttlYears = ttlYears;
    }

    public async Task<DateTime?> ProcessYearAsync(int year, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var yearRates = await _apiClient.GetDefaultExchangeRatesForYearAsync(year);
        if (!yearRates.Any())
        {
            _logger.LogWarning("No rates returned for CZ year {Year}", year);
            return null;
        }

        var grouped = yearRates
            .Where(r => !string.IsNullOrWhiteSpace(r.ValidFor))
            .Select(r => new
            {
                DateTime.ParseExact(r.ValidFor, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date,
                Rate = r.ToExchangeRateEntity()
            })
            .GroupBy(x => x.Date, x => x.Rate)
            .ToList();

        foreach (var grp in grouped)
        {
            ct.ThrowIfCancellationRequested();
            var date = grp.Key;
            var key = CacheKeyHelper.RatesKey(Language.CZ, date);

            var existing = await _cacheRepository.GetRatesDictionaryAsync(key);
            if (existing is not null && existing.Any())
            {
                continue;
            }

            var dict = grp.ToDictionary(r => r.SourceCurrency.Code, r => r);
            await _cacheRepository.SetRatesAsync(key, dict, CacheKeyHelper.DefaultTtlYears(_ttlYears));
        }

        _logger.LogInformation("Backfilled CZ year {Year} -> {Days} days cached", year, grouped.Count);

        return grouped.Min(g => g.Key);
    }
}