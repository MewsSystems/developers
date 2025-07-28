using Mews.ExchangeRateUpdater.Application.Exceptions;
using Mews.ExchangeRateUpdater.Application.Interfaces;
using Mews.ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Mews.ExchangeRateUpdater.Application.UseCases;

public class FetchExchangeRatesUseCase : IFetchExchangeRatesUseCase
{
    private readonly ICnbService _service;
    private readonly IExchangeRateRepository _repository;
    private readonly ILogger<FetchExchangeRatesUseCase> _logger;

    public FetchExchangeRatesUseCase(ICnbService service, IExchangeRateRepository repository, ILogger<FetchExchangeRatesUseCase> logger)
    {
        _service = service;
        _repository = repository;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken ct, bool forceUpdate = false)
    {
        var today = DateTime.UtcNow.Date;
        _logger.LogInformation("Executing fetch use case for date {Date} (forceUpdate: {Force})", today, forceUpdate);

        var alreadyExists = await _repository.HasRatesForDateAsync(today, ct);
        if (alreadyExists && !forceUpdate)
        {
            _logger.LogWarning("Rates already exist for {Date}, skipping fetch.", today);
            throw new RatesAlreadyExistException(today);
        }

        _logger.LogDebug("Fetching latest exchange rates from CNB");
        var rates = await _service.GetLatestRatesAsync(ct);

        if (!rates.Any())
        {
            _logger.LogError("Fetched exchange rates are empty");
            throw new EmptyRatesFetchedException();
        }

        _logger.LogInformation("Upserting {Count} rates for date {Date}", rates.Count(), today);
        await _repository.UpsertRatesAsync(rates, today, ct);
    }
}