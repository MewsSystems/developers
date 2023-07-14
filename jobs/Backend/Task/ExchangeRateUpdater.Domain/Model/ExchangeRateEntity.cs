namespace ExchangeRateUpdater.Domain.Model
{
    public class ExchangeRateEntity
    {
        public string CurrencyCode { get; set; } = string.Empty;

        public int Amount { get; set; }

        public decimal Rate { get; set; }

        public DateOnly ValidFor { get; set; }
    }
}
