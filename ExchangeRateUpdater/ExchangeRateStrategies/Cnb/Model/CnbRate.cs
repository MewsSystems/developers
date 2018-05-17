namespace ExchangeRateUpdater.ExchangeRateStrategies.Cnb.Model
{
    public class CnbRate
    {
        public string CurrencyCode { get; }
        public int Amount { get; }
        public decimal Rate { get; }

        public CnbRate(string currencyCode, int amount, decimal rate)
        {
            CurrencyCode = currencyCode;
            Amount = amount;
            Rate = rate;
        }

        public override string ToString() => $"{Amount} {CurrencyCode} = {Rate} CZK";

        public decimal BaseRate => Rate / Amount;

        protected bool Equals(CnbRate other) => string.Equals(CurrencyCode, other.CurrencyCode);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CnbRate) obj);
        }

        public override int GetHashCode() => CurrencyCode != null ? CurrencyCode.GetHashCode() : 0;
    }
}
