using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Application.Interfaces;

public interface ICzechNationalBankExchangeRateClient
{
	Task<IEnumerable<ExchangeRate>> FetchExchangeRates(string language, DateOnly date);
}