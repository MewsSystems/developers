using System.Collections.Generic;

namespace ExchangeRateUpdated.CnbApi.Model
{
    public partial class ExRateDailyResponse
    {
        public ICollection<ExRateDailyRest>? Rates { get; set; }
    }
}
