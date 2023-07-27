using ExchangeRateProvider;

namespace ExchangeRatesProvider.Models
{
    public class RatesViewModel
    {
        public List<ExchangeRate> rates { get; set; }
        public Currency sourceCurrency { get; set; }
    }
}
