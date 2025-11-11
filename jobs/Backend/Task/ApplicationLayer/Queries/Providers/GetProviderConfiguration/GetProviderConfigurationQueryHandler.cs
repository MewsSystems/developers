using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRateProviders;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Providers.GetProviderConfiguration;

public class GetProviderConfigurationQueryHandler
    : IQueryHandler<GetProviderConfigurationQuery, ExchangeRateProviderDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProviderConfigurationQueryHandler> _logger;

    public GetProviderConfigurationQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetProviderConfigurationQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ExchangeRateProviderDetailDto?> Handle(
        GetProviderConfigurationQuery request,
        CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.ExchangeRateProviders
            .GetByIdAsync(request.ProviderId, cancellationToken);

        if (provider == null)
        {
            _logger.LogWarning("Provider {ProviderId} not found", request.ProviderId);
            return null;
        }

        // Get base currency
        var baseCurrency = await _unitOfWork.Currencies.GetByIdAsync(provider.BaseCurrencyId, cancellationToken);
        var baseCurrencyCode = baseCurrency?.Code ?? "UNKNOWN";

        // Map configurations
        var configurationDtos = provider.Configurations.Select(c => new ProviderConfigurationDto
        {
            Id = c.Id,
            SettingKey = c.SettingKey,
            SettingValue = c.SettingValue,
            Description = c.Description,
            Created = c.Created,
            Modified = c.Modified
        }).ToList();

        return new ExchangeRateProviderDetailDto
        {
            Id = provider.Id,
            Name = provider.Name,
            Code = provider.Code,
            Url = provider.Url,
            BaseCurrencyId = provider.BaseCurrencyId,
            BaseCurrencyCode = baseCurrencyCode,
            RequiresAuthentication = provider.RequiresAuthentication,
            ApiKeyVaultReference = provider.ApiKeyVaultReference,
            IsActive = provider.IsActive,
            Status = provider.Status.ToString(),
            ConsecutiveFailures = provider.ConsecutiveFailures,
            LastSuccessfulFetch = provider.LastSuccessfulFetch,
            LastFailedFetch = provider.LastFailedFetch,
            Created = provider.Created,
            Modified = provider.Modified,
            Configurations = configurationDtos
        };
    }
}
