using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.ExchangeRateProviders.DeactivateProvider;

/// <summary>
/// Handler for deactivating an exchange rate provider.
/// </summary>
public class DeactivateProviderCommandHandler : ICommandHandler<DeactivateProviderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeactivateProviderCommandHandler> _logger;

    public DeactivateProviderCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeactivateProviderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeactivateProviderCommand request,
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

            provider.Deactivate();

            await _unitOfWork.ExchangeRateProviders.UpdateAsync(provider, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Deactivated provider {ProviderCode} (ID: {ProviderId})",
                provider.Code,
                provider.Id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error deactivating provider {ProviderId}",
                request.ProviderId);

            return Result.Failure($"An error occurred while deactivating the provider: {ex.Message}");
        }
    }
}
