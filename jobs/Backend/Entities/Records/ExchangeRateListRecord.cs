using Entities.Concrete;
namespace Entities.Records
{
    public class ExchangeRateListRecord
    {
        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }
        public ExchangeRateListRecord(IEnumerable<ExchangeRate> exchangeRates)
        {
            ExchangeRates = exchangeRates;
        }
    }
}
