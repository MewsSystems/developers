using CsvHelper.Configuration.Attributes;

namespace ExchangeRateUpdater.Integration
{
    public class CnbExchangeRate
    {
        [Name("země")]
        public string CountryName { get; set; }

        [Name("měna")]
        public string CurrencyName { get; set; }

        [Name("množství")]
        public int Count { get; set; }

        [Name("kód")]
        public string CurrencyCode { get; set; }

        [Name("kurz")]
        public decimal Rate { get; set; }
    }
}