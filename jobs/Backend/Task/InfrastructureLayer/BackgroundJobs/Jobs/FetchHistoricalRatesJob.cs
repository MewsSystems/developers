using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Common.Interfaces;
using ConfigurationLayer.Interface;
using DomainLayer.Interfaces.Persistence;
using DomainLayer.Interfaces.Services;
using InfrastructureLayer.ExternalServices.Discovery;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfrastructureLayer.BackgroundJobs.Jobs;

/// <summary>
/// Background job that fetches historical exchange rates for all providers on startup.
/// </summary>
public class FetchHistoricalRatesJob
{
    private readonly IProviderDiscoveryService _providerDiscovery;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IConfigurationService _configService;
    private readonly IExchangeRatesNotificationService _notificationService;
    private readonly ILogger<FetchHistoricalRatesJob> _logger;

    public FetchHistoricalRatesJob(
        IProviderDiscoveryService providerDiscovery,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IDateTimeProvider dateTimeProvider,
        IConfigurationService configService,
        IExchangeRatesNotificationService notificationService,
        ILogger<FetchHistoricalRatesJob> logger)
    {
        _providerDiscovery = providerDiscovery;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
        _configService = configService;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting historical rates fetch for all providers");

        // Get configuration from ConfigurationService (cache -> db -> appsettings)
        var historicalDataDays = await _configService.GetValueAsync<int>("HistoricalDataDays", 90);

        var providers = _providerDiscovery.DiscoverProviders();
        var endDate = _dateTimeProvider.Today;
        var startDate = endDate.AddDays(-historicalDataDays);

        foreach (var providerAdapter in providers)
        {
            long? fetchLogId = null;
            try
            {
                _logger.LogInformation(
                    "Fetching historical rates for provider {ProviderCode}",
                    providerAdapter.ProviderCode);

                // Get provider from database
                var provider = await _unitOfWork.ExchangeRateProviders
                    .GetByCodeAsync(providerAdapter.ProviderCode, cancellationToken);

                if (provider == null)
                {
                    _logger.LogWarning(
                        "Provider {ProviderCode} not found in database. Skipping.",
                        providerAdapter.ProviderCode);
                    continue;
                }

                // Check if provider is active
                if (!provider.IsActive)
                {
                    _logger.LogInformation(
                        "Provider {ProviderCode} is inactive. Skipping.",
                        providerAdapter.ProviderCode);
                    continue;
                }

                // Start fetch log tracking
                fetchLogId = await _unitOfWork.FetchLogs.StartFetchLogAsync(
                    provider.Id,
                    requestedBy: null,
                    cancellationToken);

                // Fetch historical rates (the existing providers handle date ranges internally)
                var response = await providerAdapter.FetchHistoricalRatesAsync(cancellationToken);

                if (!response.IsSuccess)
                {
                    _logger.LogError(
                        "Failed to fetch historical rates for {ProviderCode}: {Error}",
                        providerAdapter.ProviderCode,
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
                    continue;
                }

                // Process the rates (use command to handle bulk insert)
                _logger.LogInformation(
                    "Successfully fetched {Count} rates for {ProviderCode}",
                    response.Rates.Count,
                    providerAdapter.ProviderCode);

                // Track totals across all date groups
                int totalInserted = 0;
                int totalUpdated = 0;
                var errors = new List<string>();

                // Convert provider rates to command DTOs
                var groupByDate = response.Rates.GroupBy(x => x.ValidDate);
                foreach (var group in groupByDate)
                {
                    var rateItems = group.Select(r => new ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates.ExchangeRateItemDto(
                        r.SourceCurrencyCode,
                        r.TargetCurrencyCode,
                        r.Rate,
                        r.Multiplier)).ToList();
                    // Execute bulk upsert command
                    var upsertCommand = new ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates.BulkUpsertExchangeRatesCommand(
                        provider.Id,
                        group.Key,
                        rateItems);

                    var upsertResult = await _mediator.Send(upsertCommand, cancellationToken);

                    if (upsertResult.IsSuccess)
                    {
                        _logger.LogInformation(
                            "Bulk upsert for {ProviderCode} on {Date}: {Inserted} inserted, {Updated} updated, {Unchanged} unchanged",
                            providerAdapter.ProviderCode,
                            group.Key,
                            upsertResult.Value!.RatesInserted,
                            upsertResult.Value.RatesUpdated,
                            upsertResult.Value.RatesUnchanged);

                        totalInserted += upsertResult.Value.RatesInserted;
                        totalUpdated += upsertResult.Value.RatesUpdated;
                    }
                    else
                    {
                        _logger.LogError(
                            "Failed to upsert rates for {ProviderCode} on {Date}: {Error}",
                            providerAdapter.ProviderCode,
                            group.Key,
                            upsertResult.Error);

                        errors.Add($"Date {group.Key}: {upsertResult.Error}");
                    }
                }

                // Complete fetch log once after processing all date groups
                if (fetchLogId.HasValue)
                {
                    if (errors.Count == 0)
                    {
                        // All successful
                        await _unitOfWork.FetchLogs.CompleteFetchLogAsync(
                            fetchLogId.Value,
                            "Success",
                            ratesImported: totalInserted,
                            ratesUpdated: totalUpdated,
                            errorMessage: null,
                            cancellationToken);

                        provider.RecordSuccessfulFetch(totalInserted + totalUpdated);
                    }
                    else if (totalInserted > 0 || totalUpdated > 0)
                    {
                        // Partial success
                        await _unitOfWork.FetchLogs.CompleteFetchLogAsync(
                            fetchLogId.Value,
                            "PartialSuccess",
                            ratesImported: totalInserted,
                            ratesUpdated: totalUpdated,
                            errorMessage: string.Join("; ", errors),
                            cancellationToken);

                        provider.RecordSuccessfulFetch(totalInserted + totalUpdated);
                    }
                    else
                    {
                        // Complete failure
                        await _unitOfWork.FetchLogs.CompleteFetchLogAsync(
                            fetchLogId.Value,
                            "Failed",
                            ratesImported: 0,
                            ratesUpdated: 0,
                            errorMessage: string.Join("; ", errors),
                            cancellationToken);

                        provider.RecordFailedFetch(string.Join("; ", errors));
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error fetching historical rates for {ProviderCode}",
                    providerAdapter.ProviderCode);

                // Complete fetch log with error if it was started
                if (fetchLogId.HasValue)
                {
                    try
                    {
                        await _unitOfWork.FetchLogs.CompleteFetchLogAsync(
                            fetchLogId.Value,
                            "Failed",
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
                        "FetchHistoricalRatesJob",
                        $"Unexpected error fetching historical rates for {providerAdapter.ProviderCode}",
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

        _logger.LogInformation("Completed historical rates fetch for all providers");

        // Notify all connected SignalR clients about the update
        try
        {
            await _notificationService.NotifyHistoricalRatesUpdatedAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SignalR notification after historical rates fetch");
        }
    }
}
