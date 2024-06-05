namespace ExchangeRateProvider.Models;

public interface IBankApiClient
{
    Task<IEnumerable<BankCurrencyRate>> GetDailyExchangeRatesAsync(CancellationToken cancellationToken = default);
}