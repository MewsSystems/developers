using System.Collections.Generic;


namespace ExchangesRates.Infrastructure.External.CNB.Dtos
{
    public class ExRateDailyResponse
    {
        public List<ExRateDailyRest> Rates { get; set; } = new();
    }
}
