using ExchangeRateUpdater.Infrastructure.Models.CzechNationalBank;

namespace ExchangeRateUpdater.Infrastructure.Clients
{
    public interface ICzechNationalBankApiClient
    {
        Task<CzechNationalBankExchangeRatesResponse?> GetExchangeRatesAsync(DateTime? date = null, string lang = "EN");
    }
}
