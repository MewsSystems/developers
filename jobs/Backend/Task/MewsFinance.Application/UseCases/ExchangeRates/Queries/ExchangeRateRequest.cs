namespace MewsFinance.Application.UseCases.ExchangeRates.Queries
{
    public class ExchangeRateRequest
    {
        public IEnumerable<string> CurrencyCodes { get; set; } = Enumerable.Empty<string>();
    }
}
