using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.ExchangeRateProviders.UpdateProviderConfiguration;

public class UpdateProviderConfigurationCommandHandler
    : ICommandHandler<UpdateProviderConfigurationCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateProviderConfigurationCommandHandler> _logger;

    public UpdateProviderConfigurationCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateProviderConfigurationCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateProviderConfigurationCommand request,
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
            // Update basic info if provided
            if (request.Name != null || request.Url != null)
            {
                var name = request.Name ?? provider.Name;
                var url = request.Url ?? provider.Url;
                provider.UpdateInfo(name, url);

                _logger.LogInformation(
                    "Updated info for provider {ProviderCode}: Name={Name}, Url={Url}",
                    provider.Code,
                    name,
                    url);
            }

            // Update authentication if provided
            if (request.RequiresAuthentication.HasValue)
            {
                var requiresAuth = request.RequiresAuthentication.Value;
                var apiKeyRef = request.ApiKeyVaultReference ?? provider.ApiKeyVaultReference;

                provider.UpdateAuthentication(requiresAuth, apiKeyRef);

                _logger.LogInformation(
                    "Updated authentication for provider {ProviderCode}: RequiresAuth={RequiresAuth}",
                    provider.Code,
                    requiresAuth);
            }

            // Persist the updated provider
            await _unitOfWork.ExchangeRateProviders.UpdateAsync(provider, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Successfully updated configuration for provider {ProviderCode}",
                provider.Code);

            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid configuration update for provider {ProviderId}",
                request.ProviderId);

            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error updating configuration for provider {ProviderId}",
                request.ProviderId);

            return Result.Failure($"Failed to update provider configuration: {ex.Message}");
        }
    }
}
