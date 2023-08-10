using ExchangeRatesGetterWorkerService.Context;
using ExchangeRatesGetterWorkerService.Models;


namespace ExchangeRatesGetterWorkerService.Helpers
{
    public class DbHelper
    {
        public static void WriteRates(AppDbContext context, ExchangeRateData[] rates)
        {
            context.Database.EnsureCreated();


            context.Rates.AddRange(rates);
            context.SaveChanges();
        }

        public static List<ExchangeRateData> ReadAllRates(AppDbContext context)
        {
            return context.Rates.ToList();
        }
    }
}
