using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Api.Endpoints.ExchangeRates.Get;

public class GetExchangeRatesResponse
{
    public IEnumerable<ExchangeRate> ExchangeRates { get; set; } = default!;
}
