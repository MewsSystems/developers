namespace ExchangeRateUpdater
{
    public class CNBExchangeRateRecord
    {
        public CNBExchangeRateRecord(string isoCode, decimal exchangeRate, int amount)
        {
            Amount = amount;
            IsoCode = isoCode;
            _exchangeRate = exchangeRate;
        }

        private decimal _exchangeRate;

        public int Amount { get; private set; }

        public string IsoCode { get; private set; }

        public decimal ExchangeRate => _exchangeRate / Amount;
    }
}
