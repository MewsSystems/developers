using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.ExchangeRates.GetExchangeRateHistory;

public class GetExchangeRateHistoryQueryHandler
    : IQueryHandler<GetExchangeRateHistoryQuery, IEnumerable<ExchangeRateHistoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetExchangeRateHistoryQueryHandler> _logger;

    public GetExchangeRateHistoryQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetExchangeRateHistoryQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<ExchangeRateHistoryDto>> Handle(
        GetExchangeRateHistoryQuery request,
        CancellationToken cancellationToken)
    {
        // Get currencies
        var sourceCurrency = await _unitOfWork.Currencies.GetByCodeAsync(request.SourceCurrencyCode, cancellationToken);
        var targetCurrency = await _unitOfWork.Currencies.GetByCodeAsync(request.TargetCurrencyCode, cancellationToken);

        if (sourceCurrency == null)
        {
            _logger.LogWarning("Source currency {Code} not found", request.SourceCurrencyCode);
            return Enumerable.Empty<ExchangeRateHistoryDto>();
        }

        if (targetCurrency == null)
        {
            _logger.LogWarning("Target currency {Code} not found", request.TargetCurrencyCode);
            return Enumerable.Empty<ExchangeRateHistoryDto>();
        }

        // Get historical rates
        var rates = await _unitOfWork.ExchangeRates.GetHistoryAsync(
            sourceCurrency.Id,
            targetCurrency.Id,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        // Filter by provider if specified
        if (request.ProviderId.HasValue)
        {
            rates = rates.Where(r => r.ProviderId == request.ProviderId.Value);
        }

        // Get provider information for mapping
        var providers = await _unitOfWork.ExchangeRateProviders.GetAllAsync(cancellationToken);
        var providerDict = providers.ToDictionary(p => p.Id, p => (p.Name, p.Code));

        // Map to DTOs
        var result = rates.Select(r =>
        {
            var (providerName, providerCode) = providerDict.TryGetValue(r.ProviderId, out var providerInfo)
                ? providerInfo
                : ("Unknown", "UNKNOWN");

            return new ExchangeRateHistoryDto
            {
                ValidDate = r.ValidDate,
                Rate = r.Rate,
                Multiplier = r.Multiplier,
                EffectiveRate = r.EffectiveRate,
                ProviderId = r.ProviderId,
                ProviderName = providerName,
                ProviderCode = providerCode,
                SourceCurrencyCode = request.SourceCurrencyCode,
                BaseCurrencyCode = request.SourceCurrencyCode, // Base currency is the source
                TargetCurrencyCode = request.TargetCurrencyCode
            };
        })
        .OrderBy(r => r.ValidDate)
        .ToList();

        _logger.LogInformation(
            "Retrieved {Count} historical rates for {Source}/{Target} from {Start} to {End}",
            result.Count,
            request.SourceCurrencyCode,
            request.TargetCurrencyCode,
            request.StartDate,
            request.EndDate);

        return result;
    }
}
