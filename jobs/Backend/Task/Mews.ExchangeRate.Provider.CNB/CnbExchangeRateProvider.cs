using Mews.ExchangeRate.Domain;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Mews.ExchangeRate.Provider.CNB;
internal sealed class CnbExchangeRateProvider : 
    IProvideExchangeRates,
    IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("OK"));
    }

    public Task<IEnumerable<Domain.ExchangeRate>> GetExchangeRatesForCurrenciesAsync(IEnumerable<Currency> currencies)
    {
        return Task.FromResult(Enumerable.Empty<Domain.ExchangeRate>());
    }
}
