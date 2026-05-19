namespace ExchangeRateUpdater.Model.Dto
{
    public class RateDataDto
    {
        public List<CnbRate> Rates { get; set; }
    }

    public class CnbRate
    {
        public string ValidFor { get; set; }
        public int Order { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }
}
