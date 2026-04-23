namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Clients;

public interface ICzechNationalBankClient
{
    Task<string> GetDailyRatesAsync(CancellationToken cancellationToken = default);
}