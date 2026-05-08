using ExchangeRateUpdater.Application.DTOs;

namespace ExchangeRateUpdater.Application.Interfaces;

public interface IExchangeRateService
{
    Task<RatesResponse> GetAllRatesAsync(CancellationToken cancellationToken = default);
    Task<ExchangeRateDto?> GetRateByCurrencyAsync(string currencyCode, CancellationToken cancellationToken = default);
}
