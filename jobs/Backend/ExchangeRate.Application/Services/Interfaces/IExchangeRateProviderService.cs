using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Services.Interfaces
{
    public interface IExchangeRateProviderService
    {
        Task<ExchangeRateProviderResultDTO> GetExchangeRatesByDate(DateTime date, CurrencyDTO currencyDTO);
        Task<ExchangeRateProviderResultDTO?> GetExchangeRatesByDate(ExchangeRatesDTO currency);
        Task<ExchangeRateProviderResultDTO> GetExchangeRates();
        Task<ExchangeRateProviderResultDTO> GetExchangeRates(CurrenciesDTO currenciesDTO);
    }
}