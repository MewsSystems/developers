namespace ExchangeRateUpdater.Application.GetExchangeRates;

public interface ICzechNationalBankExchangeRateClient
{
    Task<string?> GetAsync(DateOnly date);
}