using ExchangeRate.Service.Enums;

namespace ExchangeRate.Service.Interfaces;

public interface IExchangeRateService
{
    Task<IEnumerable<Models.ExchangeRate>> GetExchangeRates(ProviderSource source);
}