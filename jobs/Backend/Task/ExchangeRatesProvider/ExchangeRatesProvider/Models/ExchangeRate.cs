using System.Text.Json.Serialization;

namespace ExchangeRateProvider
{
    public class ExchangeRate
    {

        public string validFor { get; set; }
        public int order { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public int amount { get; set; }
        public string currencyCode { get; set; }
        public decimal rate { get; set; }
        public ExchangeRate(string ValidFor, int Order, string Country, string Currency, int Amount, string CurrencyCode, decimal Rate)
        {
            validFor = ValidFor;
            order = Order;
            country = Country;
            currency = Currency;
            amount = Amount;
            currencyCode = CurrencyCode;
            rate = Rate;
        }

        public override string ToString()
        {
            var sourceCurrency = "CZK";
            return $"{sourceCurrency}/{currencyCode}={rate}";
        }
    }
}
