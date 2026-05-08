using System.Collections.Generic;

namespace ExchangeRateUpdater.API.CNB.Model.Responses
{
    public class ExRateDailyResponse
    {
        public IEnumerable<ExRateDailyRest> Rates { get; set; }
    }
}
