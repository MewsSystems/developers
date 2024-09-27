using Data;

namespace Core.Services.CzechNationalBank.Interfaces;

public interface IExchangeRateService
{
    Task<List<ExchangeRate>> GetExchangeRates(List<Currency> currencies);
}
