using ExchangeRateUpdater.ApiClient.Common;

namespace ExchangeRateUpdater.ApiClient.Client.ExchangeDaily
{
    public class ExchangeDailyRequest
    {
        public DateTime DateTime { get; set; }
        public Language Language { get; set; }
    }
}
