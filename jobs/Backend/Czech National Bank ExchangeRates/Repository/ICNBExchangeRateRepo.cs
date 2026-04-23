using Czech_National_Bank_ExchangeRates.Models;

namespace Czech_National_Bank_ExchangeRates.Repository
{
    public interface ICNBExchangeRateRepo
    {
        Task<ExchangeRates> GetExhangeRateData(string dateString);
    }
}