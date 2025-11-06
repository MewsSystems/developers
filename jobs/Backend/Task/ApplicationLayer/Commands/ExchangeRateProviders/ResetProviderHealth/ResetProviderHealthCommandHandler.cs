using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.ExchangeRateProviders.ResetProviderHealth;

/// <summary>
/// Handler for resetting a provider's health status.
/// </summary>
public class ResetProviderHealthCommandHandler : ICommandHandler<ResetProviderHealthCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ResetProviderHealthCommandHandler> _logger;

    public ResetProviderHealthCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ResetProviderHealthCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        ResetProviderHealthCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var provider = await _unitOfWork.ExchangeRateProviders
                .GetByIdAsync(request.ProviderId, cancellationToken);

            if (provider == null)
            {
                return Result.Failure($"Provider with ID {request.ProviderId} not found.");
            }

            provider.ResetHealthStatus();

            await _unitOfWork.ExchangeRateProviders.UpdateAsync(provider, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Reset health status for provider {ProviderCode} (ID: {ProviderId})",
                provider.Code,
                provider.Id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error resetting health for provider {ProviderId}",
                request.ProviderId);

            return Result.Failure($"An error occurred while resetting provider health: {ex.Message}");
        }
    }
}
