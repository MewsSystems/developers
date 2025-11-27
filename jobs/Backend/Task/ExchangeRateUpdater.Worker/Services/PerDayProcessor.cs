namespace ExchangeRateUpdater.Worker.Services;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Utils;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

public interface IPerDayProcessor
{
    Task ProcessDateAsync(DateTime date, Domain.Enums.Language language, CancellationToken ct);
}

public class PerDayProcessor : IPerDayProcessor
{
    private readonly IExchangeRateApiClient _apiClient;
    private readonly ICacheRepository _cacheRepository;
    private readonly ILogger<PerDayProcessor> _logger;
    private readonly int _ttlYears;

    public PerDayProcessor(IExchangeRateApiClient apiClient, ICacheRepository cacheRepository,
        ILogger<PerDayProcessor> logger, int ttlYears = 4)
    {
        Ensure.Argument.NotNull(apiClient, nameof(apiClient));
        Ensure.Argument.NotNull(cacheRepository, nameof(cacheRepository));
        Ensure.Argument.NotNull(logger, nameof(logger));

        _apiClient = apiClient;
        _cacheRepository = cacheRepository;
        _logger = logger;
        _ttlYears = ttlYears;
    }

    public async Task ProcessDateAsync(DateTime date, Domain.Enums.Language language, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var key = CacheKeyHelper.RatesKey(language, date);
        var existing = await _cacheRepository.GetRatesDictionaryAsync(key);
        if (existing is not null && existing.Any())
        {
            _logger.LogDebug("Cache hit for {Language} on {Date}, skipping fetch", language, date);
            return;
        }

        var rates = (await _apiClient.GetExchangeRatesAsync(date, language)).Select(x => x.ToExchangeRateEntity()).ToList();
        if (!rates.Any())
        {
            _logger.LogDebug("No rates for {Language} on {Date}", language, date);
            return;
        }

        var dict = rates.ToDictionary(r => r.SourceCurrency.Code, r => r);
        await _cacheRepository.SetRatesAsync(key, dict, CacheKeyHelper.DefaultTtlYears(_ttlYears));
    }
}