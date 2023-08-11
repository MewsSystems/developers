using ExchangeRatesGetterWorkerService.Context;
using ExchangeRatesGetterWorkerService.Models;
using Microsoft.EntityFrameworkCore;

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

        public static void ClearTable(AppDbContext context)
        {
            context.Database.EnsureCreated();
            context.Database.ExecuteSqlRaw($"TRUNCATE TABLE [{AppDbContext.TABLE_NAME}]");
        }
    }
}
