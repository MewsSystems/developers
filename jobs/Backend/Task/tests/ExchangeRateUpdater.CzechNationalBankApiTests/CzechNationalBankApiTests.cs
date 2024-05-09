namespace ExchangeRateUpdater.CzechNationalBankApiTests
{
    public class CzechNationalBankApiTests
    {
        private readonly ExchangeRateProvider _sut;

        public CzechNationalBankApiTests()
        {
            _sut = new ExchangeRateProvider();
        }
    }
}