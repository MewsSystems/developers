using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRateProviders;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Providers.GetProviderById;

/// <summary>
/// Handler for retrieving a provider by ID.
/// </summary>
public class GetProviderByIdQueryHandler : IQueryHandler<GetProviderByIdQuery, ExchangeRateProviderDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProviderByIdQueryHandler> _logger;

    public GetProviderByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetProviderByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ExchangeRateProviderDetailDto?> Handle(
        GetProviderByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting provider by ID: {ProviderId}", request.ProviderId);

        var provider = await _unitOfWork.ExchangeRateProviders
            .GetByIdAsync(request.ProviderId, cancellationToken);

        if (provider == null)
        {
            _logger.LogDebug("Provider {ProviderId} not found", request.ProviderId);
            return null;
        }

        _logger.LogDebug("Found provider {ProviderCode}", provider.Code);

        // Get the currency info
        var currency = await _unitOfWork.Currencies
            .GetByIdAsync(provider.BaseCurrencyId, cancellationToken);

        if (currency == null)
        {
            _logger.LogWarning(
                "Base currency {CurrencyId} not found for provider {ProviderCode}",
                provider.BaseCurrencyId,
                provider.Code);
        }

        // Map to DTO
        return new ExchangeRateProviderDetailDto
        {
            Id = provider.Id,
            Name = provider.Name,
            Code = provider.Code,
            Url = provider.Url,
            BaseCurrencyId = provider.BaseCurrencyId,
            BaseCurrencyCode = currency?.Code ?? "UNKNOWN",
            RequiresAuthentication = provider.RequiresAuthentication,
            ApiKeyVaultReference = provider.ApiKeyVaultReference,
            IsActive = provider.IsActive,
            Status = provider.Status.ToString(),
            ConsecutiveFailures = provider.ConsecutiveFailures,
            LastSuccessfulFetch = provider.LastSuccessfulFetch,
            LastFailedFetch = provider.LastFailedFetch,
            Created = provider.Created,
            Modified = provider.Modified,
            Configurations = provider.Configurations.Select(c => new ProviderConfigurationDto
            {
                Id = c.Id,
                ProviderId = c.ProviderId,
                SettingKey = c.SettingKey,
                SettingValue = c.SettingValue,
                Description = c.Description,
                Created = c.Created,
                Modified = c.Modified
            }).ToList()
        };
    }
}
