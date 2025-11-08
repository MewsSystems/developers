using ApplicationLayer.Common.Interfaces;
using ConfigurationLayer.Interface;
using DomainLayer.Interfaces.Persistence;
using DomainLayer.Interfaces.Services;
using Hangfire;
using InfrastructureLayer.ExternalServices.Discovery;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfrastructureLayer.BackgroundJobs.Jobs;

/// <summary>
/// Background job that fetches the latest exchange rates for a specific provider.
/// Implements smart retry logic when data already exists.
/// </summary>
public class FetchLatestRatesJob
{
    private readonly IProviderDiscoveryService _providerDiscovery;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IConfigurationService _configService;
    private readonly IExchangeRatesNotificationService _notificationService;
    private readonly ILogger<FetchLatestRatesJob> _logger;

    public FetchLatestRatesJob(
        IProviderDiscoveryService providerDiscovery,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IDateTimeProvider dateTimeProvider,
        IConfigurationService configService,
        IExchangeRatesNotificationService notificationService,
        ILogger<FetchLatestRatesJob> logger)
    {
        _providerDiscovery = providerDiscovery;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
        _configService = configService;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task ExecuteAsync(string providerCode, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting latest rates fetch for provider {ProviderCode}", providerCode);

        long? fetchLogId = null;
        try
        {
            // Get provider adapter
            var providerAdapter = _providerDiscovery.GetProviderByCode(providerCode);
            if (providerAdapter == null)
            {
                _logger.LogError("Provider adapter {ProviderCode} not found", providerCode);
                return;
            }

            // Get provider from database
            var provider = await _unitOfWork.ExchangeRateProviders
                .GetByCodeAsync(providerCode, cancellationToken);

            if (provider == null)
            {
                _logger.LogError("Provider {ProviderCode} not found in database", providerCode);
                return;
            }

            // Check if provider can fetch
            try
            {
                provider.EnsureCanFetch();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    "Provider {ProviderCode} cannot fetch: {Message}",
                    providerCode,
                    ex.Message);
                return;
            }

            // Start fetch log tracking
            fetchLogId = await _unitOfWork.FetchLogs.StartFetchLogAsync(
                provider.Id,
                requestedBy: null,
                cancellationToken);

            // Fetch latest rates
            var response = await providerAdapter.FetchLatestRatesAsync(cancellationToken);

            if (!response.IsSuccess)
            {
                _logger.LogError(
                    "Failed to fetch latest rates for {ProviderCode}: {Error}",
                    providerCode,
                    response.ErrorMessage);

                // Complete fetch log with failure
                await _unitOfWork.FetchLogs.CompleteFetchLogAsync(
                    fetchLogId.Value,
                    "Failed",
                    ratesImported: 0,
                    ratesUpdated: 0,
                    errorMessage: response.ErrorMessage,
                    cancellationToken);

                provider.RecordFailedFetch(response.ErrorMessage);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return;
            }

            // Check if rates for this ValidDate already exist
            var existingRates = await _unitOfWork.ExchangeRates
                .GetByProviderAndDateAsync(provider.Id, response.Rates.Max(x => x.ValidDate), cancellationToken);

            if (existingRates.Any())
            {
                var lastUpdate = existingRates.Max(r => r.Modified ?? r.Created);
                var timeSinceUpdate = _dateTimeProvider.UtcNow - lastUpdate;

                // Get configuration from ConfigurationService (cache -> db -> appsettings)
                var recentDataThresholdHours = await _configService.GetValueAsync<int>("RecentDataThresholdHours", 2);
                var defaultRetryDelayMinutes = await _configService.GetValueAsync<int>("DefaultRetryDelayMinutes", 30);

                // If data is recent, schedule a retry to give provider time to update
                if (timeSinceUpdate < TimeSpan.FromHours(recentDataThresholdHours))
                {
                    var retryDelay = TimeSpan.FromMinutes(defaultRetryDelayMinutes);

                    BackgroundJob.Schedule<RetryFailedFetchesJob>(
                        job => job.ExecuteAsync(provider.Id, response.Rates.Max(x => x.ValidDate), 1, cancellationToken),
                        retryDelay);

                    _logger.LogInformation(
                        "Rates for {ValidDate} already exist for {ProviderCode} (updated {TimeSince} ago). Scheduled retry in {Delay}",
                        response.Rates.Max(x => x.ValidDate),
                        providerCode,
                        timeSinceUpdate,
                        retryDelay);

                    return;
                }

                _logger.LogInformation(
                    "Rates for {ValidDate} exist but are stale ({TimeSince} old). Updating.",
                    response.Rates.Max(x => x.ValidDate),
                    timeSinceUpdate);
            }

            // Process rates (new or stale data)
            _logger.LogInformation(
                "Processing {Count} rates for {ProviderCode} with ValidDate {ValidDate}",
                response.Rates.Count,
                providerCode,
                response.Rates.Max(x => x.ValidDate));

            // Convert provider rates to command DTOs
            var rateItems = response.Rates.Select(r => new ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates.ExchangeRateItemDto(
                r.SourceCurrencyCode,
                r.TargetCurrencyCode,
                r.Rate,
                r.Multiplier)).ToList();

            // Execute bulk upsert command
            var upsertCommand = new ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates.BulkUpsertExchangeRatesCommand(
                provider.Id,
                response.Rates.Max(x => x.ValidDate),
                rateItems);

            var upsertResult = await _mediator.Send(upsertCommand, cancellationToken);

            if (upsertResult.IsSuccess)
            {
                _logger.LogInformation(
                    "Bulk upsert for {ProviderCode}: {Inserted} inserted, {Updated} updated, {Unchanged} unchanged",
                    providerCode,
                    upsertResult.Value!.RatesInserted,
                    upsertResult.Value.RatesUpdated,
                    upsertResult.Value.RatesUnchanged);

                // Complete fetch log with success
                if (fetchLogId.HasValue)
                {
                    await _unitOfWork.FetchLogs.CompleteFetchLogAsync(
                        fetchLogId.Value,
                        "Success",
                        ratesImported: upsertResult.Value.RatesInserted,
                        ratesUpdated: upsertResult.Value.RatesUpdated,
                        errorMessage: null,
                        cancellationToken);
                }

                // Update provider health
                provider.RecordSuccessfulFetch(upsertResult.Value.RatesInserted + upsertResult.Value.RatesUpdated);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Successfully processed latest rates for {ProviderCode}",
                    providerCode);

                // Notify all connected SignalR clients about the update
                try
                {
                    await _notificationService.NotifyLatestRatesUpdatedAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending SignalR notification after latest rates fetch for {ProviderCode}", providerCode);
                }
            }
            else
            {
                _logger.LogError(
                    "Failed to upsert rates for {ProviderCode}: {Error}",
                    providerCode,
                    upsertResult.Error);

                // Complete fetch log with failure
                if (fetchLogId.HasValue)
                {
                    await _unitOfWork.FetchLogs.CompleteFetchLogAsync(
                        fetchLogId.Value,
                        "Failed",
                        ratesImported: 0,
                        ratesUpdated: 0,
                        errorMessage: upsertResult.Error,
                        cancellationToken);
                }

                provider.RecordFailedFetch(upsertResult.Error);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error fetching latest rates for {ProviderCode}",
                providerCode);

            // Complete fetch log with error if it was started
            if (fetchLogId.HasValue)
            {
                try
                {
                    await _unitOfWork.FetchLogs.CompleteFetchLogAsync(
                        fetchLogId.Value,
                        "Error",
                        ratesImported: 0,
                        ratesUpdated: 0,
                        errorMessage: ex.Message,
                        cancellationToken);
                }
                catch
                {
                    // Ignore errors in error handling
                }
            }

            // Log error to database
            try
            {
                await _unitOfWork.ErrorLogs.LogErrorAsync(
                    "Error",
                    "FetchLatestRatesJob",
                    $"Unexpected error fetching latest rates for {providerCode}",
                    ex.ToString(),
                    ex.StackTrace,
                    cancellationToken);
            }
            catch
            {
                // Ignore errors in error logging
            }
        }
    }
}
