using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;
using DomainLayer.Interfaces.Persistence;
using DomainLayer.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.ExchangeRates.GetCurrentExchangeRates;

/// <summary>
/// Handler for retrieving current exchange rates.
/// Returns the most recent rates for all active currency pairs.
/// </summary>
public class GetCurrentExchangeRatesQueryHandler : IQueryHandler<GetCurrentExchangeRatesQuery, IEnumerable<CurrentExchangeRateDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<GetCurrentExchangeRatesQueryHandler> _logger;

    public GetCurrentExchangeRatesQueryHandler(
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ILogger<GetCurrentExchangeRatesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<IEnumerable<CurrentExchangeRateDto>> Handle(
        GetCurrentExchangeRatesQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting current exchange rates");

        // Get all active providers
        var providers = await _unitOfWork.ExchangeRateProviders.GetAllAsync(cancellationToken);
        var activeProviders = providers.Where(p => p.IsActive).ToList();

        if (!activeProviders.Any())
        {
            _logger.LogWarning("No active providers found");
            return Enumerable.Empty<CurrentExchangeRateDto>();
        }

        // Get all currencies for mapping
        var currencies = await _unitOfWork.Currencies.GetAllAsync(cancellationToken);
        var currencyDict = currencies.ToDictionary(c => c.Id, c => c.Code);

        // Get latest rates for each provider
        var currentRates = new List<CurrentExchangeRateDto>();
        var today = _dateTimeProvider.Today;

        foreach (var provider in activeProviders)
        {
            var rates = await _unitOfWork.ExchangeRates
                .GetByProviderAndDateAsync(provider.Id, today, cancellationToken);

            foreach (var rate in rates)
            {
                var baseCurrencyCode = currencyDict.TryGetValue(rate.BaseCurrencyId, out var baseCode)
                    ? baseCode : "UNKNOWN";
                var targetCurrencyCode = currencyDict.TryGetValue(rate.TargetCurrencyId, out var targetCode)
                    ? targetCode : "UNKNOWN";

                var lastUpdated = rate.Modified ?? rate.Created;
                var daysOld = (int)(_dateTimeProvider.UtcNow - lastUpdated).TotalDays;

                currentRates.Add(new CurrentExchangeRateDto
                {
                    ProviderCode = provider.Code,
                    BaseCurrencyCode = baseCurrencyCode,
                    TargetCurrencyCode = targetCurrencyCode,
                    Rate = rate.Rate,
                    Multiplier = rate.Multiplier,
                    EffectiveRate = rate.Rate / rate.Multiplier,
                    ValidDate = rate.ValidDate,
                    LastUpdated = lastUpdated,
                    DaysOld = daysOld
                });
            }
        }

        _logger.LogDebug("Retrieved {Count} current exchange rates from {ProviderCount} providers",
            currentRates.Count, activeProviders.Count);

        return currentRates;
    }
}
