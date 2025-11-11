using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.ExchangeRateProviders.DeleteProvider;

public class DeleteProviderCommandHandler
    : ICommandHandler<DeleteProviderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProviderCommandHandler> _logger;

    public DeleteProviderCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteProviderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteProviderCommand request,
        CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.ExchangeRateProviders
            .GetByIdAsync(request.ProviderId, cancellationToken);

        if (provider == null)
        {
            _logger.LogWarning("Provider {ProviderId} not found", request.ProviderId);
            return Result.Failure($"Provider with ID {request.ProviderId} not found.");
        }

        try
        {
            // Check if provider has exchange rates
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var existingRates = await _unitOfWork.ExchangeRates
                .GetByProviderAndDateAsync(request.ProviderId, today, cancellationToken);

            if (existingRates.Any() && !request.Force)
            {
                _logger.LogWarning(
                    "Cannot delete provider {ProviderCode} because it has associated exchange rates",
                    provider.Code);

                return Result.Failure(
                    $"Cannot delete provider '{provider.Code}' because it has associated exchange rates. " +
                    "Use Force=true to delete anyway (this will also delete all associated exchange rates).");
            }

            // If force deletion, first delete all associated exchange rates
            if (request.Force && existingRates.Any())
            {
                _logger.LogWarning(
                    "Force deleting provider {ProviderCode} and {Count} associated exchange rates",
                    provider.Code,
                    existingRates.Count());

                foreach (var rate in existingRates)
                {
                    await _unitOfWork.ExchangeRates.DeleteAsync(rate, cancellationToken);
                }
            }

            // Delete provider
            await _unitOfWork.ExchangeRateProviders.DeleteAsync(provider, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Successfully deleted provider {ProviderCode}",
                provider.Code);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error deleting provider {ProviderId}",
                request.ProviderId);

            return Result.Failure($"Failed to delete provider: {ex.Message}");
        }
    }
}
