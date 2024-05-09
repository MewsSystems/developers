namespace ExchangeRateUpdater.CzechNationalBankApiTests
{
    public class CzechNationalBankApiTests
    {
        private readonly CzechNationalBankExchangeRateProvider _sut;

        public CzechNationalBankApiTests()
        {
            _sut = new CzechNationalBankExchangeRateProvider();
        }
    }
}