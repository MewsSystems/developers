using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.Common.Interfaces;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.ExchangeRateProviders.TriggerManualFetch;

public class TriggerManualFetchCommandHandler
    : ICommandHandler<TriggerManualFetchCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBackgroundJobService _backgroundJobService;
    private readonly ILogger<TriggerManualFetchCommandHandler> _logger;

    public TriggerManualFetchCommandHandler(
        IUnitOfWork unitOfWork,
        IBackgroundJobService backgroundJobService,
        ILogger<TriggerManualFetchCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _backgroundJobService = backgroundJobService;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(
        TriggerManualFetchCommand request,
        CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.ExchangeRateProviders
            .GetByIdAsync(request.ProviderId, cancellationToken);

        if (provider == null)
        {
            _logger.LogWarning("Provider {ProviderId} not found", request.ProviderId);
            return Result.Failure<string>($"Provider with ID {request.ProviderId} not found.");
        }

        try
        {
            // Check if provider can fetch
            provider.EnsureCanFetch();

            // Enqueue background job to fetch rates immediately
            var jobId = _backgroundJobService.EnqueueFetchRatesJob(provider.Code);

            _logger.LogInformation(
                "Manual fetch triggered for provider {ProviderCode}. Job ID: {JobId}",
                provider.Code,
                jobId);

            return Result.Success(jobId);
        }
        catch (Exception ex) when (
            ex is DomainLayer.Exceptions.ProviderNotActiveException ||
            ex is DomainLayer.Exceptions.ProviderQuarantinedException)
        {
            _logger.LogWarning(
                ex,
                "Cannot trigger fetch for provider {ProviderCode}: {Message}",
                provider.Code,
                ex.Message);

            return Result.Failure<string>(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error triggering manual fetch for provider {ProviderId}",
                request.ProviderId);

            return Result.Failure<string>($"Failed to trigger manual fetch: {ex.Message}");
        }
    }
}
