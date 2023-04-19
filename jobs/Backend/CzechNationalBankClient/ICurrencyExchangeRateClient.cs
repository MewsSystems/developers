using CzechNationalBankClient.Model;

namespace CzechNationalBankClient
{
    public interface ICurrencyExchangeRateClient
    {
        Task<IEnumerable<CnbExchangeRate>> GetCurrencyExchangeRatesAsync(string date, string language);
        Task<IEnumerable<CnbExchangeRate>> GetOtherCurrencyExchangeRatesAsync(string month, string language);
    }
}
