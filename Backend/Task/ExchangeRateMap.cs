using CsvHelper.Configuration;

namespace ExchangeRateUpdater
{
    public class ExchangeRateMap : ClassMap<FxRateCnb>
    {
        public ExchangeRateMap()
        {
            Map(m => m.TargetCurrency).Name("Code");
            Map(m => m.Rate).Name("Rate");
            Map(m => m.Amount).Name("Amount");
        }
    }
}
