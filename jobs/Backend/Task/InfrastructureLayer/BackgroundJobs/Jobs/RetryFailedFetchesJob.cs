using ConfigurationLayer.Interface;
using DomainLayer.Interfaces.Persistence;
using DomainLayer.Interfaces.Services;
using Hangfire;
using InfrastructureLayer.ExternalServices.Discovery;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfrastructureLayer.BackgroundJobs.Jobs;

/// <summary>
/// Background job that retries fetching exchange rates after a delay.
/// Used when data already exists to give providers time to update.
/// </summary>
public class RetryFailedFetchesJob
{
    private readonly IProviderDiscoveryService _providerDiscovery;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IConfigurationService _configService;
    private readonly ILogger<RetryFailedFetchesJob> _logger;

    public RetryFailedFetchesJob(
        IProviderDiscoveryService providerDiscovery,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IConfigurationService configService,
        ILogger<RetryFailedFetchesJob> logger)
    {
        _providerDiscovery = providerDiscovery;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _configService = configService;
        _logger = logger;
    }

    public async Task ExecuteAsync(
        int providerId,
        DateOnly validDate,
        int retryAttempt,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Retrying fetch for provider {ProviderId}, ValidDate {ValidDate}, attempt {Attempt}",
            providerId,
            validDate,
            retryAttempt);

        try
        {
            // Get provider
            var provider = await _unitOfWork.ExchangeRateProviders
                .GetByIdAsync(providerId, cancellationToken);

            if (provider == null)
            {
                _logger.LogError("Provider {ProviderId} not found", providerId);
                return;
            }

            // Get provider adapter
            var providerAdapter = _providerDiscovery.GetProviderByCode(provider.Code);
            if (providerAdapter == null)
            {
                _logger.LogError("Provider adapter {ProviderCode} not found", provider.Code);
                return;
            }

            // Fetch latest rates again
            var response = await providerAdapter.FetchLatestRatesAsync(cancellationToken);

            if (!response.IsSuccess)
            {
                _logger.LogError(
                    "Retry {Attempt} failed for {ProviderCode}: {Error}",
                    retryAttempt,
                    provider.Code,
                    response.ErrorMessage);

                // Get configuration from ConfigurationService (cache -> db -> appsettings)
                var maxRetries = await _configService.GetValueAsync<int>("MaxRetries", 3);
                var defaultRetryDelayMinutes = await _configService.GetValueAsync<int>("DefaultRetryDelayMinutes", 30);

                // Schedule another retry if not exceeded max attempts
                if (retryAttempt < maxRetries)
                {
                    // Exponential backoff: 30m, 60m, 120m
                    var delay = TimeSpan.FromMinutes(defaultRetryDelayMinutes * Math.Pow(2, retryAttempt - 1));

                    BackgroundJob.Schedule<RetryFailedFetchesJob>(
                        job => job.ExecuteAsync(providerId, validDate, retryAttempt + 1, cancellationToken),
                        delay);

                    _logger.LogInformation(
                        "Scheduled retry {NextAttempt} for {ProviderCode} in {Delay}",
                        retryAttempt + 1,
                        provider.Code,
                        delay);
                }
                else
                {
                    _logger.LogWarning(
                        "Max retries ({MaxRetries}) reached for {ProviderCode}. Giving up until next scheduled fetch.",
                        maxRetries,
                        provider.Code);
                }

                return;
            }

            // Process the fetched rates
            _logger.LogInformation(
                "Retry {Attempt} successful for {ProviderCode}. Processing {Count} rates.",
                retryAttempt,
                provider.Code,
                response.Rates.Count);

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
                    "Retry {Attempt} bulk upsert for {ProviderCode}: {Inserted} inserted, {Updated} updated, {Unchanged} unchanged",
                    retryAttempt,
                    provider.Code,
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
                    "Failed to upsert rates for {ProviderCode} on retry {Attempt}: {Error}",
                    provider.Code,
                    retryAttempt,
                    upsertResult.Error);

                provider.RecordFailedFetch(upsertResult.Error);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error during retry {Attempt} for provider {ProviderId}",
                retryAttempt,
                providerId);
        }
    }
}
