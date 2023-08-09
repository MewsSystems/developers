using ExchangeRatesGetterWorkerService.Context;
using ExchangeRatesGetterWorkerService.Models;


namespace ExchangeRatesGetterWorkerService.Helpers
{
    public class DbHelper
    {
        public static void InitDb(AppDbContext context)
        {
            context.Database.EnsureCreated();

            ExchangeRateData rateDatat = new ExchangeRateData();
            rateDatat.SourceCurrency = "eur";
            rateDatat.TargetCurrency = "usd";
            rateDatat.Value = (decimal)0.11;
            rateDatat.ValidTill = DateTime.Now.AddDays(1);
            rateDatat.ValidFrom = DateTime.Now;

            context.Rates.Add(rateDatat);
            context.SaveChanges();
        }

        public static List<ExchangeRateData> GetRates(AppDbContext context)
        {
            return context.Rates.ToList();
        }
    }
}
