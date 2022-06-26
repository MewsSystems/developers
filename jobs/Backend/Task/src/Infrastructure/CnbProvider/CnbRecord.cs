using CsvHelper.Configuration.Attributes;

namespace Infrastructure.CnbProvider
{
    public class CnbRecord
    {
        [Name("země")]
        public string Country { get; set; }

        [Name("měna")]
        public string Currency { get; set; }

        [Name("množství")]
        public int Amount { get; set; }

        [Name("kód")]
        public string Code { get; set; }

        [Name("kurz")]
        public decimal Rate { get; set; }
    }
}
