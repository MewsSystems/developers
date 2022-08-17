namespace ExchangeRateUpdater.Model
{
    public interface ICurrencyDetails
    {
        decimal Factor { get; set; }

        decimal Rate { get; set; }

        Currency Currency { get; set; }
    }

    public class CnbCurrencyDetails : ICurrencyDetails
    {
        public string Country { get; set; }

        public string CurrecyExtended { get; set; }

        public decimal Factor { get; set; }

        public Currency Currency { get; set; }

        public decimal Rate { get; set; }
    }
}
