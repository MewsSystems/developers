namespace CzechNationalBankApi
{
    public interface ICzechBankApiService
    {
        Task<IEnumerable<CzechExchangeItemDto>> GetExchangeRatesAsync();
    }
}
