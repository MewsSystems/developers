using System.Collections.Generic;

namespace ExchangeRateUpdater.Models
{
    public class ExchangeApiResponse
    {
        public List<FxRate> Rates { get; set; }
    }
}

