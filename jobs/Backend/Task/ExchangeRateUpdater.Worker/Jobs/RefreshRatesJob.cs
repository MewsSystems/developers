namespace ExchangeRateUpdater.Worker.Jobs;

using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Quartz;

[DisallowConcurrentExecution]
public class RefreshRatesJob : IJob
{
    private readonly IExchangeRateApiClient apiClient;
    private readonly ICacheRepository cacheRepository;
    private readonly ILogger<RefreshRatesJob> logger;

    public RefreshRatesJob(IExchangeRateApiClient apiClient, ICacheRepository cacheRepository, ILogger<RefreshRatesJob> logger)
    {
        this.apiClient = apiClient;
        this.cacheRepository = cacheRepository;
        this.logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            logger.LogInformation("Starting RefreshRatesJob at {Time}", DateTimeOffset.UtcNow);
            var rates = await apiClient.GetExchangeRatesAsync(null, null);
            var list = rates?.ToList() ?? new System.Collections.Generic.List<object>();
            cacheRepository.SetCache("rates:latest", list);
            // store updated timestamp separately for scalar metrics
            cacheRepository.SetCache("rates:latest:updatedAt", DateTimeOffset.UtcNow);
            logger.LogInformation("RefreshRatesJob completed - {Count} rates cached", list.Count);
        }
        catch (Exception e)
        {
            logger.LogError(e, "RefreshRatesJob failed");
            throw;
        }
    }
}
