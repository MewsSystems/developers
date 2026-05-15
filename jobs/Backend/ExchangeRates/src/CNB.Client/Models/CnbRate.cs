using ExchangeRates.Domain;

namespace CNB.Client.Models
{
    public record CnbRate
    {
        public int Amount { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }
        public int Order { get; set; }
        public decimal Rate { get; set; }
        public string ValidFor { get; set; }

        public ExchangeRate ToDomain()
        {

            //TODO: add validation here
            return new ExchangeRate(new Currency(CurrencyCode), new Currency("CZK"), (decimal)Rate / Amount);
        }
    }
}
