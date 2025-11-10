using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.Common.Exceptions;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.ExchangeRates.ConvertCurrency;

public class ConvertCurrencyQueryHandler
    : IQueryHandler<ConvertCurrencyQuery, CurrencyConversionResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ConvertCurrencyQueryHandler> _logger;

    public ConvertCurrencyQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<ConvertCurrencyQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CurrencyConversionResult> Handle(
        ConvertCurrencyQuery request,
        CancellationToken cancellationToken)
    {
        // Get currencies
        var sourceCurrency = await _unitOfWork.Currencies.GetByCodeAsync(request.SourceCurrencyCode, cancellationToken);
        var targetCurrency = await _unitOfWork.Currencies.GetByCodeAsync(request.TargetCurrencyCode, cancellationToken);

        if (sourceCurrency == null)
        {
            throw new NotFoundException($"Source currency '{request.SourceCurrencyCode}' not found.");
        }

        if (targetCurrency == null)
        {
            throw new NotFoundException($"Target currency '{request.TargetCurrencyCode}' not found.");
        }

        // Get the exchange rate
        DomainLayer.Aggregates.ExchangeRateAggregate.ExchangeRate? exchangeRate;

        if (request.Date.HasValue)
        {
            // Get rate for specific date
            var rates = await _unitOfWork.ExchangeRates.GetHistoryAsync(
                sourceCurrency.Id,
                targetCurrency.Id,
                request.Date.Value,
                request.Date.Value,
                cancellationToken);

            if (request.ProviderId.HasValue)
            {
                exchangeRate = rates.FirstOrDefault(r => r.ProviderId == request.ProviderId.Value);
            }
            else
            {
                // Get the most recently created rate for that date
                exchangeRate = rates.OrderByDescending(r => r.Created).FirstOrDefault();
            }
        }
        else
        {
            // Get latest rate
            exchangeRate = await _unitOfWork.ExchangeRates.GetLatestRateAsync(
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
        }

        // If no direct rate found, try inverse rate (target -> source)
        bool useInverseRate = false;
        if (exchangeRate == null)
        {
            _logger.LogInformation(
                "No direct rate found for {Source}->{Target}, trying inverse rate",
                request.SourceCurrencyCode,
                request.TargetCurrencyCode);

            if (request.Date.HasValue)
            {
                // Get inverse rate for specific date
                var inverseRates = await _unitOfWork.ExchangeRates.GetHistoryAsync(
                    targetCurrency.Id,
                    sourceCurrency.Id,
                    request.Date.Value,
                    request.Date.Value,
                    cancellationToken);

                if (request.ProviderId.HasValue)
                {
                    exchangeRate = inverseRates.FirstOrDefault(r => r.ProviderId == request.ProviderId.Value);
                }
                else
                {
                    exchangeRate = inverseRates.OrderByDescending(r => r.Created).FirstOrDefault();
                }
            }
            else
            {
                // Get latest inverse rate
                exchangeRate = await _unitOfWork.ExchangeRates.GetLatestRateAsync(
                    targetCurrency.Id,
                    sourceCurrency.Id,
                    cancellationToken);

                // Filter by provider if specified
                if (request.ProviderId.HasValue && exchangeRate != null && exchangeRate.ProviderId != request.ProviderId.Value)
                {
                    var today = DateOnly.FromDateTime(DateTime.UtcNow);
                    var inverseRates = await _unitOfWork.ExchangeRates.GetHistoryAsync(
                        targetCurrency.Id,
                        sourceCurrency.Id,
                        today.AddDays(-90),
                        today,
                        cancellationToken);

                    exchangeRate = inverseRates
                        .Where(r => r.ProviderId == request.ProviderId.Value)
                        .OrderByDescending(r => r.ValidDate)
                        .ThenByDescending(r => r.Created)
                        .FirstOrDefault();
                }
            }

            if (exchangeRate != null)
            {
                useInverseRate = true;
                _logger.LogInformation(
                    "Using inverse rate {Target}->{Source} for conversion",
                    request.TargetCurrencyCode,
                    request.SourceCurrencyCode);
            }
        }

        if (exchangeRate == null)
        {
            throw new NotFoundException(
                $"No exchange rate found for {request.SourceCurrencyCode}/{request.TargetCurrencyCode}" +
                (request.Date.HasValue ? $" on {request.Date.Value}" : "") +
                (request.ProviderId.HasValue ? $" from provider {request.ProviderId.Value}" : ""));
        }

        // Get provider name
        var provider = await _unitOfWork.ExchangeRateProviders.GetByIdAsync(exchangeRate.ProviderId, cancellationToken);
        var providerName = provider?.Name ?? "Unknown";

        // Perform conversion (use inverse method if we're using an inverse rate)
        var convertedAmount = useInverseRate
            ? exchangeRate.ConvertAmountInverse(request.Amount)
            : exchangeRate.ConvertAmount(request.Amount);

        _logger.LogInformation(
            "Converted {Amount} {Source} to {Result} {Target} using rate {Rate}/{Multiplier} from {Provider} (ValidDate: {Date})",
            request.Amount,
            request.SourceCurrencyCode,
            convertedAmount,
            request.TargetCurrencyCode,
            exchangeRate.Rate,
            exchangeRate.Multiplier,
            providerName,
            exchangeRate.ValidDate);

        return new CurrencyConversionResult(
            request.Amount,
            convertedAmount,
            request.SourceCurrencyCode,
            request.TargetCurrencyCode,
            exchangeRate.Rate,
            exchangeRate.Multiplier,
            exchangeRate.EffectiveRate,
            exchangeRate.ValidDate,
            exchangeRate.ProviderId,
            providerName);
    }
}
