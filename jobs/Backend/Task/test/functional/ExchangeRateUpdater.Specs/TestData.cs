using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.ExchangeRateApiServiceClient;

namespace ExchangeRateUpdater.Specs
{
    public class TestData
    {
        public static IDictionary<string, GetExchangeRatesResponse> GetExchangeRatesResponses { get; private set; }
        public static IEnumerable<string> Lines { get; set; }

        public static void Reset()
        {
            GetExchangeRatesResponses = new Dictionary<string, GetExchangeRatesResponse>();
            Lines = Enumerable.Empty<string>();
        }
    }
}