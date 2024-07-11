namespace Czech_National_Bank_ExchangeRates.Models
{
    public class ExchangeRates
    {
        public List<Rates> Rates { get; set; }
    }

    public class Rates
    {
        public string ValidFor { get; set; }
        public string CurrencyCode { get; set; }
        public string Country {  get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public decimal Rate { get; set; }
        public int Order {  get; set; }
    }

}
