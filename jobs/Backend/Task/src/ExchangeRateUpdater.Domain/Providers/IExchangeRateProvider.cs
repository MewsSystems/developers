using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Providers;

public interface IExchangeRateProvider
{
    string Name { get; }
    string DefaultLanguage { get; }
    string DefaultCurrency { get; }
    Task<ExchangeRate[]> FetchAllCurrentAsync();
    Task<ExchangeRate[]> FetchByDateAsync(DateTime date);
}