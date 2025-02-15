using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Services.Interfaces
{
    public interface IExchangeRateProviderService
    {
        Task<Dictionary<string, ExchangeRateProviderDTO>> GetExchangeRatesByDate(DateTime date, CurrencyDTO currencyDTO);
        Task<Dictionary<string, ExchangeRateProviderDTO>> GetExchangeRates();
    }
}