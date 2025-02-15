using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Services.Interfaces
{
    public interface IExchangeRateService
    {
        Task<ExchangeRatesBankDTO> GetDailyExchangeRates();
        Task<ExchangeRatesBankDTO> GetExchangeRatesByDay(DateTime date);
    }
}
