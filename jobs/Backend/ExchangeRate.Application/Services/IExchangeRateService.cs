using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Services
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRateBankDTO>> GetDailyExchangeRates();
        Task<List<ExchangeRateBankDTO>> GetExchangeRatesByDay(DateTime date);
    }
}
