namespace ExchangeRateUpdater.Worker.Jobs;

using Application.Common.Interfaces;
using Application.Common.Mappings;
using Domain.Common;
using Domain.Enums;
using Domain.Repositories;
using ExchangeRateUpdater.Application.Common.Utils;
using Quartz;

[DisallowConcurrentExecution]
public class DailyExchangeRatesRefreshJob : IJob
{
    private readonly IExchangeRateApiClient _apiClient;
    private readonly ICacheRepository _cacheRepository;
    private readonly ILogger<DailyExchangeRatesRefreshJob> _logger;

    public DailyExchangeRatesRefreshJob(IExchangeRateApiClient apiClient,
        ICacheRepository cacheRepository,
        ILogger<DailyExchangeRatesRefreshJob> logger)
    {
        Ensure.Argument.NotNull(apiClient, nameof(apiClient));
        Ensure.Argument.NotNull(cacheRepository, nameof(cacheRepository));
        Ensure.Argument.NotNull(logger, nameof(logger));
        _apiClient = apiClient;
        _cacheRepository = cacheRepository;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Starting RefreshRatesJob at {Time}", DateTimeOffset.UtcNow);

            await SetDailyExchangeRatesInCacheByLanguage(Language.CZ);
            
            await SetDailyExchangeRatesInCacheByLanguage(Language.EN);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "RefreshRatesJob failed");
            throw;
        }
    }

    private async Task SetDailyExchangeRatesInCacheByLanguage(Language language)
    {
        var rates = (await _apiClient.GetExchangeRatesAsync(null, null)).Select(r => r.ToExchangeRateEntity()).ToList();
        if (!rates.Any())
        {
            _logger.LogWarning("No rates returned from API");
        }

        var today = DateTime.UtcNow.Date;
        var dateKey = CacheKeyHelper.RatesKey(language, today);
        var ratesDict = rates.ToDictionary(r => r.SourceCurrency.Code, r => r);

        await _cacheRepository.SetRatesAsync(dateKey, ratesDict, TimeSpan.FromDays(365));

        _logger.LogInformation("RefreshRatesJob completed - {Count} rates cached for {Languague} - {Date}", ratesDict.Count, language, today);
    }
}