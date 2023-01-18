namespace Domain
{
    public class CzechNationalBankExchangeRate
    {
        public string Country { get; set; } = null!;

        public string Currency { get; set; } = null!;

        public int Amount { get; set; }

        public string Code { get; set; } = null!;

        public decimal Rate { get; set; }
    }
}
