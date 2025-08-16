using ExchangeRateProvider.Contract.Models;

namespace ExchangeRateProvider.Models.NationalBank
{
    public class NationalBankExchangeRate
    {
        public DateTime? ValidFor { get; set; }
        public int? Order { get; set; }
        public string? Country { get; set; }
        public string? Currency { get; set; }
        public int Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }

        public ExchangeRate ToExchangeRate()
        {
            return new ExchangeRate(
                new Currency("CZK"),
                new Currency(CurrencyCode),
                Rate);
        }
    }
}
