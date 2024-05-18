using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.API.Models.RequestModels
{
    public class GetExchangeRatesRequest
    {
        public IEnumerable<string> Currencies { get; set; }
    }
}
