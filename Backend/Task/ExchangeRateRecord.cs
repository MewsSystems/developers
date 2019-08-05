namespace ExchangeRateUpdater
{
    public class ExchangeRateRecord
    {
        public ExchangeRateRecord(string isoCode, decimal exchangeRate, int lot)
        {
            Lot = lot;
            IsoCode = isoCode;
            _exchangeRate = exchangeRate;
        }

        private decimal _exchangeRate;

        public int Lot { get; private set; }

        public string IsoCode { get; private set; }

        public decimal ExchangeRate => _exchangeRate / Lot;
    }
}