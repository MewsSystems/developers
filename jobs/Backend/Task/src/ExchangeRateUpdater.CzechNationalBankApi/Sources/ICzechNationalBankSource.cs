using ExchangeRateUpdater.Core.Models.CzechNationalBank;

namespace ExchangeRateUpdater.CzechNationalBank.Sources
{
    public interface ICzechNationalBankSource
    {
        Task<ExchangeRatesDailyDto?> GetExchangeRatesAsync();
    }
}
