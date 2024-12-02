using ExchangeRateService.Domain;

namespace ExchangeRateService.ExternalServices;

public interface ICNBClientService
{
    ValueTask<ExchangeRate[]> GetDailyExchangeRatesAsync(CancellationToken cancellationToken);
}