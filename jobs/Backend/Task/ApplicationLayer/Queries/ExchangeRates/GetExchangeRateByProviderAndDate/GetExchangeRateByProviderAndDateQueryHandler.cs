using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.ExchangeRates.GetExchangeRateByProviderAndDate;

public class GetExchangeRateByProviderAndDateQueryHandler
    : IQueryHandler<GetExchangeRateByProviderAndDateQuery, IEnumerable<ExchangeRateDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetExchangeRateByProviderAndDateQueryHandler> _logger;

    public GetExchangeRateByProviderAndDateQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetExchangeRateByProviderAndDateQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<ExchangeRateDto>> Handle(
        GetExchangeRateByProviderAndDateQuery request,
        CancellationToken cancellationToken)
    {
        // Verify provider exists
        var provider = await _unitOfWork.ExchangeRateProviders.GetByIdAsync(request.ProviderId, cancellationToken);
        if (provider == null)
        {
            _logger.LogWarning("Provider {ProviderId} not found", request.ProviderId);
            return Enumerable.Empty<ExchangeRateDto>();
        }

        // Get rates for provider and date
        var rates = await _unitOfWork.ExchangeRates.GetByProviderAndDateAsync(
            request.ProviderId,
            request.ValidDate,
            cancellationToken);

        // Get all currencies for mapping
        var currencies = await _unitOfWork.Currencies.GetAllAsync(cancellationToken);
        var currencyDict = currencies.ToDictionary(c => c.Id, c => c.Code);

        // Map to DTOs
        var result = rates.Select(r => new ExchangeRateDto
        {
            Id = r.Id,
            ProviderId = r.ProviderId,
            ProviderName = provider.Name,
            BaseCurrencyId = r.BaseCurrencyId,
            BaseCurrencyCode = currencyDict.TryGetValue(r.BaseCurrencyId, out var baseCurrency) ? baseCurrency : "UNKNOWN",
            TargetCurrencyId = r.TargetCurrencyId,
            TargetCurrencyCode = currencyDict.TryGetValue(r.TargetCurrencyId, out var targetCurrency) ? targetCurrency : "UNKNOWN",
            Rate = r.Rate,
            Multiplier = r.Multiplier,
            EffectiveRate = r.EffectiveRate,
            ValidDate = r.ValidDate,
            Created = r.Created,
            Modified = r.Modified
        }).ToList();

        _logger.LogInformation(
            "Retrieved {Count} rates for provider {ProviderCode} on {Date}",
            result.Count,
            provider.Code,
            request.ValidDate);

        return result;
    }
}
