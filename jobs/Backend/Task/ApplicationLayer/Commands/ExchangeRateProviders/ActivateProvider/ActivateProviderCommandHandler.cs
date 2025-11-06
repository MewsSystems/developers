using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.Common.Exceptions;
using DomainLayer.Common;
using DomainLayer.Exceptions;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.ExchangeRateProviders.ActivateProvider;

/// <summary>
/// Handler for activating an exchange rate provider.
/// </summary>
public class ActivateProviderCommandHandler : ICommandHandler<ActivateProviderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ActivateProviderCommandHandler> _logger;

    public ActivateProviderCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ActivateProviderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        ActivateProviderCommand request,
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

            // Domain logic handles quarantine check
            provider.Activate();

            await _unitOfWork.ExchangeRateProviders.UpdateAsync(provider, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Activated provider {ProviderCode} (ID: {ProviderId})",
                provider.Code,
                provider.Id);

            return Result.Success();
        }
        catch (ProviderQuarantinedException ex)
        {
            _logger.LogWarning(
                "Cannot activate quarantined provider {ProviderId}: {Message}",
                request.ProviderId,
                ex.Message);

            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error activating provider {ProviderId}",
                request.ProviderId);

            return Result.Failure($"An error occurred while activating the provider: {ex.Message}");
        }
    }
}
