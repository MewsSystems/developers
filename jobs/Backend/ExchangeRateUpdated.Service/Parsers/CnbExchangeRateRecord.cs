namespace ExchangeRateUpdated.Service.Parsers
{
    public class CnbExchangeRateRecord
    {
        public string Country { get; set; } = default!;
        public string Currency { get; set; } = default!;
        public int Amount { get; set; }
        public string Code { get; set; } = default!;
        public decimal Rate { get; set; }
    }
}
