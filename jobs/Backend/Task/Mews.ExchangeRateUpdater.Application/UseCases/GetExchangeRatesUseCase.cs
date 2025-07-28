using Mews.ExchangeRateUpdater.Application.Exceptions;
using Mews.ExchangeRateUpdater.Application.Interfaces;
using Mews.ExchangeRateUpdater.Domain.Interfaces;
using Mews.ExchangeRateUpdater.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Mews.ExchangeRateUpdater.Application.UseCases;

public class GetExchangeRatesUseCase : IGetExchangeRatesUseCase
{
    private readonly IExchangeRateRepository _repository;
    private readonly ILogger<GetExchangeRatesUseCase> _logger;

    public GetExchangeRatesUseCase(IExchangeRateRepository repository, ILogger<GetExchangeRatesUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<ExchangeRate>> ExecuteAsync(IEnumerable<Currency> currencies, CancellationToken ct)
    {
        var date = DateTime.UtcNow.Date;
        _logger.LogInformation("Getting exchange rates for {Count} currencies on {Date}", currencies.Count(), date);

        var hasRates = await _repository.HasRatesForDateAsync(date, ct);

        if (!hasRates)
        {
            _logger.LogWarning("No data available for date {Date}", date);
            throw new NoDataForTodayException();
        }
        
        var result = await _repository.GetRatesAsync(date, currencies, ct);
        _logger.LogDebug("Returning {Count} rates", result.Count());
        return result;
    }
}