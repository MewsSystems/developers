using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.Common.Exceptions;
using DomainLayer.Aggregates.ProviderAggregate;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;

/// <summary>
/// Handler for creating a new exchange rate provider.
/// </summary>
public class CreateExchangeRateProviderCommandHandler
    : ICommandHandler<CreateExchangeRateProviderCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateExchangeRateProviderCommandHandler> _logger;

    public CreateExchangeRateProviderCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateExchangeRateProviderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        CreateExchangeRateProviderCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Check if provider with the same code already exists
            var existingProvider = await _unitOfWork.ExchangeRateProviders
                .GetByCodeAsync(request.Code, cancellationToken);

            if (existingProvider != null)
            {
                return Result.Failure<int>($"Provider with code '{request.Code}' already exists.");
            }

            // Verify that the base currency exists
            var currency = await _unitOfWork.Currencies
                .GetByIdAsync(request.BaseCurrencyId, cancellationToken);

            if (currency == null)
            {
                return Result.Failure<int>($"Currency with ID {request.BaseCurrencyId} not found.");
            }

            // Create the provider aggregate
            var provider = ExchangeRateProvider.Create(
                request.Name,
                request.Code,
                request.Url,
                request.BaseCurrencyId,
                request.RequiresAuthentication,
                request.ApiKeyVaultReference);

            // Add to repository
            await _unitOfWork.ExchangeRateProviders.AddAsync(provider, cancellationToken);

            // Save changes (will dispatch domain events)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Query back to get the generated ID
            var savedProvider = await _unitOfWork.ExchangeRateProviders.GetByCodeAsync(request.Code, cancellationToken);
            var providerId = savedProvider?.Id ?? 0;

            _logger.LogInformation(
                "Created exchange rate provider {ProviderCode} with ID {ProviderId}",
                provider.Code,
                providerId);

            return Result.Success(providerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error creating exchange rate provider {ProviderCode}",
                request.Code);

            return Result.Failure<int>($"An error occurred while creating the provider: {ex.Message}");
        }
    }
}
