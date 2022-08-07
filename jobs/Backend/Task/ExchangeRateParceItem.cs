using CsvHelper.Configuration.Attributes;

namespace ExchangeRateUpdater
{
    public class ExchangeRateParceItem
    {
        [Name("země")]
        public string Country { get; set; }

        [Name("měna")]
        public string Currency { get; set; }

        [Name("množství")]
        public int Count { get; set; }

        [Name("kód")]
        public string Code { get; set; }

        [Name("kurz")]
        public decimal Rate { get; set; }
    }
}
