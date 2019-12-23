namespace ExchangeRateUpdater
{
    public class CnbXmlRow
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public int Amount { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Country { get; set; }
        public decimal ExchangeRateNormalized => ExchangeRate / (Amount != 0 ? Amount : 1);
    }
}