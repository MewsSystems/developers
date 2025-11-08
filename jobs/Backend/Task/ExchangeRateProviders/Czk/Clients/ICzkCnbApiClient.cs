using ExchangeRateProviders.Czk.Model;

namespace ExchangeRateProviders.Czk.Clients;

public interface ICzkCnbApiClient
{
    Task<IReadOnlyList<CnbApiExchangeRateDto>> GetDailyRatesRawAsync(CancellationToken cancellationToken = default);
}
