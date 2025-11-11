using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.Common.Interfaces;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.ExchangeRateProviders.RescheduleProvider;

/// <summary>
/// Handler for rescheduling an exchange rate provider's recurring job.
/// Updates configuration and reschedules the Hangfire job with new time/timezone.
/// </summary>
public class RescheduleProviderCommandHandler : ICommandHandler<RescheduleProviderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBackgroundJobScheduler _jobScheduler;
    private readonly ILogger<RescheduleProviderCommandHandler> _logger;

    public RescheduleProviderCommandHandler(
        IUnitOfWork unitOfWork,
        IBackgroundJobScheduler jobScheduler,
        ILogger<RescheduleProviderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _jobScheduler = jobScheduler;
        _logger = logger;
    }

    public async Task<Result> Handle(
        RescheduleProviderCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Get the provider by code
            var provider = await _unitOfWork.ExchangeRateProviders
                .GetByCodeAsync(request.ProviderCode, cancellationToken);

            if (provider == null)
            {
                _logger.LogWarning("Provider with code {ProviderCode} not found", request.ProviderCode);
                return Result.Failure($"Provider with code '{request.ProviderCode}' not found.");
            }

            // Update configuration
            provider.SetConfiguration("UpdateTime", request.UpdateTime);
            provider.SetConfiguration("TimeZone", request.TimeZone);

            // Persist changes
            await _unitOfWork.ExchangeRateProviders.UpdateAsync(provider, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Updated configuration for provider {ProviderCode}: UpdateTime={UpdateTime}, TimeZone={TimeZone}",
                provider.Code,
                request.UpdateTime,
                request.TimeZone);

            // Reschedule the background job (also refreshes configuration cache)
            await _jobScheduler.RescheduleProviderJobAsync(provider.Code, request.UpdateTime, request.TimeZone);

            _logger.LogInformation(
                "Successfully rescheduled provider {ProviderCode}",
                provider.Code);

            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid configuration for provider {ProviderCode}",
                request.ProviderCode);

            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error rescheduling provider {ProviderCode}",
                request.ProviderCode);

            return Result.Failure($"Failed to reschedule provider: {ex.Message}");
        }
    }
}
