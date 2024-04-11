namespace MewsFinance.Infrastructure.CnbFinancialClient
{
    public class CnbExchangeRateResponse
    {
        public IReadOnlyList<CnbExchangeRate> Rates { get; set; } = new List<CnbExchangeRate>();
    }
}
