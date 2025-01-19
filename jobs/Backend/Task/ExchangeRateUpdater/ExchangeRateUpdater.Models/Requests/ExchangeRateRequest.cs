using ExchangeRateUpdater.Models.Models;

namespace ExchangeRateUpdater.Models.Requests
{
    public class ExchangeRateRequest
    {
        public Currency SourceCurrency { get; set; }
        public Currency TargetCurrency { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}";
        }

    }
}
