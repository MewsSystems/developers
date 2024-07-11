using Czech_National_Bank_ExchangeRates.Models;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
        Task<ExchangeRates> GetExchangeRatesByDate(string dateString);
    }
}