using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }

    public class Rate
    {
        public string validFor { get; set; }
        public int order { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
        public decimal rate { get; set; }
    }

    public class ExchangeRateApiResponse
    {
        public List<Rate> rates { get; set; }
    }

}
