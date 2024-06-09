namespace ExchangeRateProvider.Models;

public interface IBankApiClient
{
	Task<IEnumerable<BankCurrencyRate>> GetDailyExchangeRatesAsync(DateTimeOffset? validFor = null, CancellationToken cancellationToken = default);
}