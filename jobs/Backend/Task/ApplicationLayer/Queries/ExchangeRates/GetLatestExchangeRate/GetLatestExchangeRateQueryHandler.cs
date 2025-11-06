using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.ExchangeRates.GetLatestExchangeRate;

public class GetLatestExchangeRateQueryHandler
    : IQueryHandler<GetLatestExchangeRateQuery, ExchangeRateDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetLatestExchangeRateQueryHandler> _logger;

    public GetLatestExchangeRateQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetLatestExchangeRateQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ExchangeRateDto?> Handle(
        GetLatestExchangeRateQuery request,
        CancellationToken cancellationToken)
    {
        // Get currencies
        var sourceCurrency = await _unitOfWork.Currencies.GetByCodeAsync(request.SourceCurrencyCode, cancellationToken);
        var targetCurrency = await _unitOfWork.Currencies.GetByCodeAsync(request.TargetCurrencyCode, cancellationToken);

        if (sourceCurrency == null)
        {
            _logger.LogWarning("Source currency {Code} not found", request.SourceCurrencyCode);
            return null;
        }

        if (targetCurrency == null)
        {
            _logger.LogWarning("Target currency {Code} not found", request.TargetCurrencyCode);
            return null;
        }

        // Get latest rate
        var exchangeRate = await _unitOfWork.ExchangeRates.GetLatestRateAsync(
            sourceCurrency.Id,
            targetCurrency.Id,
            cancellationToken);

        // Filter by provider if specified
        if (request.ProviderId.HasValue && exchangeRate != null && exchangeRate.ProviderId != request.ProviderId.Value)
        {
            // Need to find latest rate from specific provider
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var rates = await _unitOfWork.ExchangeRates.GetHistoryAsync(
                sourceCurrency.Id,
                targetCurrency.Id,
                today.AddDays(-90), // Look back 90 days
                today,
                cancellationToken);

            exchangeRate = rates
                .Where(r => r.ProviderId == request.ProviderId.Value)
                .OrderByDescending(r => r.ValidDate)
                .ThenByDescending(r => r.Created)
                .FirstOrDefault();
        }

        if (exchangeRate == null)
        {
            _logger.LogInformation(
                "No exchange rate found for {Source}/{Target}",
                request.SourceCurrencyCode,
                request.TargetCurrencyCode);
            return null;
        }

        // Get provider name
        var provider = await _unitOfWork.ExchangeRateProviders.GetByIdAsync(exchangeRate.ProviderId, cancellationToken);
        var providerName = provider?.Name ?? "Unknown";

        _logger.LogInformation(
            "Found latest rate for {Source}/{Target}: {Rate}/{Multiplier} from {Provider} (ValidDate: {Date})",
            request.SourceCurrencyCode,
            request.TargetCurrencyCode,
            exchangeRate.Rate,
            exchangeRate.Multiplier,
            providerName,
            exchangeRate.ValidDate);

        return new ExchangeRateDto
        {
            Id = exchangeRate.Id,
            ProviderId = exchangeRate.ProviderId,
            ProviderName = providerName,
            BaseCurrencyId = exchangeRate.BaseCurrencyId,
            BaseCurrencyCode = request.SourceCurrencyCode,
            TargetCurrencyId = exchangeRate.TargetCurrencyId,
            TargetCurrencyCode = request.TargetCurrencyCode,
            Rate = exchangeRate.Rate,
            Multiplier = exchangeRate.Multiplier,
            EffectiveRate = exchangeRate.EffectiveRate,
            ValidDate = exchangeRate.ValidDate,
            Created = exchangeRate.Created,
            Modified = exchangeRate.Modified
        };
    }
}
