using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Services
{
    public interface IExchangeRateService
    {
        Task<ExchangeRatesDTO> GetDailyExchangeRates();
        Task<ExchangeRatesDTO> GetExchangeRatesByDay(DateTime date);
    }
}
