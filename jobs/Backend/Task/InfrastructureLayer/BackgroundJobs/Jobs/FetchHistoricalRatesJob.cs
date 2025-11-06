using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
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
    private readonly ILogger<FetchHistoricalRatesJob> _logger;

    public FetchHistoricalRatesJob(
        IProviderDiscoveryService providerDiscovery,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IDateTimeProvider dateTimeProvider,
        IConfigurationService configService,
        ILogger<FetchHistoricalRatesJob> logger)
    {
        _providerDiscovery = providerDiscovery;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
        _configService = configService;
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

                // Fetch historical rates (the existing providers handle date ranges internally)
                var response = await providerAdapter.FetchHistoricalRatesAsync(cancellationToken);

                if (!response.IsSuccess)
                {
                    _logger.LogError(
                        "Failed to fetch historical rates for {ProviderCode}: {Error}",
                        providerAdapter.ProviderCode,
                        response.ErrorMessage);

                    provider.RecordFailedFetch(response.ErrorMessage);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    continue;
                }

                // Process the rates (use command to handle bulk insert)
                _logger.LogInformation(
                    "Successfully fetched {Count} rates for {ProviderCode}",
                    response.Rates.Count,
                    providerAdapter.ProviderCode);

                // Convert provider rates to command DTOs
                var rateItems = response.Rates.Select(r => new ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates.ExchangeRateItemDto(
                    r.SourceCurrencyCode,
                    r.TargetCurrencyCode,
                    r.Rate,
                    r.Multiplier)).ToList();

                // Execute bulk upsert command
                var upsertCommand = new ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates.BulkUpsertExchangeRatesCommand(
                    provider.Id,
                    response.ValidDate,
                    rateItems);

                var upsertResult = await _mediator.Send(upsertCommand, cancellationToken);

                if (upsertResult.IsSuccess)
                {
                    _logger.LogInformation(
                        "Bulk upsert for {ProviderCode}: {Inserted} inserted, {Updated} updated, {Unchanged} unchanged",
                        providerAdapter.ProviderCode,
                        upsertResult.Value!.RatesInserted,
                        upsertResult.Value.RatesUpdated,
                        upsertResult.Value.RatesUnchanged);

                    // Update provider health
                    provider.RecordSuccessfulFetch(upsertResult.Value.RatesInserted + upsertResult.Value.RatesUpdated);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    _logger.LogError(
                        "Failed to upsert rates for {ProviderCode}: {Error}",
                        providerAdapter.ProviderCode,
                        upsertResult.Error);

                    provider.RecordFailedFetch(upsertResult.Error);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error fetching historical rates for {ProviderCode}",
                    providerAdapter.ProviderCode);
            }
        }

        _logger.LogInformation("Completed historical rates fetch for all providers");
    }
}
