namespace ExchangeRateUpdater.Domain.DTO
{
    /// <summary>
    /// Represents json returned by Cnb API
    /// </summary>
    public class CnbExchangeRates
    {
        public CnbExchangeRates()
        {
            Rates = [];
        }

        public IEnumerable<CnbExchangeRate> Rates { get; set; }
    }

    public class CnbExchangeRate
    {
        public DateTimeOffset ValidFor { get; set; }

        public int Order {  get; set; }

        public string? Country { get; set; }

        public string? Currency {  get; set; }

        public int Amount { get; set; }

        public string? CurrencyCode { get; set; }

        public decimal Rate { get; set; }
    }
}
