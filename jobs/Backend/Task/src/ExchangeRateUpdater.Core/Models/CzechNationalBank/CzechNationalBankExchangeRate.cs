namespace ExchangeRateUpdater.Core.Models.CzechNationalBank
{
    public class CzechNationalBankExchangeRate : ExchangeRate
    {
        private const int DECIMAL_PLACES = 2;
        private const string CURRENCY_CODE = "CZK";

        public CzechNationalBankExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value) : base(sourceCurrency, targetCurrency, value)
        {
        }

        public CzechNationalBankExchangeRate(int amount, decimal rate, string currencyCode) : base(new Currency(currencyCode), new Currency(CURRENCY_CODE), Math.Round(amount / rate, DECIMAL_PLACES))
        {
        }
    }
}
