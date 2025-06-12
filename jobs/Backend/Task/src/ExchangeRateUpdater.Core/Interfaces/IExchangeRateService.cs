using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using NodaTime;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateService
{
    Task<ExchangeRateResponse> GetExchangeRateAsync(string sourceCurrency,
        string targetCurrency,
        LocalDate? date = null,
        CancellationToken cancellationToken = default);

    Task<BatchExchangeRateResponse> GetBatchExchangeRatesAsync(BatchRateRequest request,
        CancellationToken cancellationToken = default);
}