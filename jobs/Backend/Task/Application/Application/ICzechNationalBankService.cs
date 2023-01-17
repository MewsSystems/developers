using Domain;

namespace Application
{
    public interface ICzechNationalBankService
    {
        public IEnumerable<CzechNationalBankExchangeRate>? GetRates();
    }
}