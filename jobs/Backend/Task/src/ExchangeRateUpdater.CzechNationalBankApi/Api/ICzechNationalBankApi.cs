using ExchangeRateUpdater.Core.Models.CzechNationalBank;

namespace ExchangeRateUpdater.CzechNationalBank.Api
{
    public interface ICzechNationalBankApi
    {
        Task<ExchangeRatesDailyDto?> GetExchangeRatesAsync();
    }
}
