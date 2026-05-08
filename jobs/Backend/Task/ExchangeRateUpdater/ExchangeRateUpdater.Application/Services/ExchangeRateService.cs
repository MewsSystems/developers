using ExchangeRateUpdater.Application.DTOs;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExchangeRateUpdater.Application.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateRepository _repository;
    private readonly ILogger<ExchangeRateService> _logger;

    public ExchangeRateService(IExchangeRateRepository repository, ILogger<ExchangeRateService>? logger = null)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? NullLogger<ExchangeRateService>.Instance;
    }

    public async Task<RatesResponse> GetAllRatesAsync(CancellationToken cancellationToken = default)
    {
        var ratesDict = await _repository.GetCurrentRatesAsync(cancellationToken);
        var dtos = ratesDict.Values
            .OrderBy(r => r.CurrencyCode)
            .Select(r => new ExchangeRateDto(r.CurrencyCode, r.RateToCZK))
            .ToList();

        return new RatesResponse(dtos, dtos.Count, DateTime.UtcNow);
    }

    public async Task<ExchangeRateDto?> GetRateByCurrencyAsync(string currencyCode, CancellationToken cancellationToken = default)
    {
        var rates = await _repository.GetCurrentRatesAsync(cancellationToken);
        return rates.TryGetValue(currencyCode.ToUpperInvariant(), out var rate)
            ? new ExchangeRateDto(rate.CurrencyCode, rate.RateToCZK)
            : null;
    }
}
