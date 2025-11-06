using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRateProviders;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Providers.GetProviderHealth;

public class GetProviderHealthQueryHandler
    : IQueryHandler<GetProviderHealthQuery, ProviderHealthDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProviderHealthQueryHandler> _logger;

    public GetProviderHealthQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetProviderHealthQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ProviderHealthDto?> Handle(
        GetProviderHealthQuery request,
        CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.ExchangeRateProviders
            .GetByIdAsync(request.ProviderId, cancellationToken);

        if (provider == null)
        {
            _logger.LogWarning("Provider {ProviderId} not found", request.ProviderId);
            return null;
        }

        return new ProviderHealthDto
        {
            ProviderId = provider.Id,
            ProviderCode = provider.Code,
            ProviderName = provider.Name,
            Status = provider.Status.ToString(),
            IsHealthy = provider.IsHealthy,
            ConsecutiveFailures = provider.ConsecutiveFailures,
            LastSuccessfulFetch = provider.LastSuccessfulFetch,
            LastFailedFetch = provider.LastFailedFetch,
            TimeSinceLastSuccess = provider.TimeSinceLastSuccessfulFetch,
            TimeSinceLastFailure = provider.TimeSinceLastFailedFetch
        };
    }
}
