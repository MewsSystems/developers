using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Services
{
    public interface IExchangeRateService
    {
        Task<ExchangeRatesBankDTO> GetDailyExchangeRates();
        Task<ExchangeRatesBankDTO> GetExchangeRatesByDay(DateTime date);
    }
}
