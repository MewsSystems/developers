namespace ExchangesRates.Infrastructure.External.CNB.Dtos
{
    public class CnbExRatesResponse
    {
        public List<CnbExRate> Rates { get; set; } = new();
    }
}
