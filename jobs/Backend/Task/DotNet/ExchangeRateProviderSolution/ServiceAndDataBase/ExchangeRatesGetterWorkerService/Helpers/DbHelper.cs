using ExchangeRatesGetterWorkerService.Context;
using ExchangeRatesGetterWorkerService.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesGetterWorkerService.Helpers
{
    public class DbHelper
    {
        private static ILogger _logger;

        public static void Init(ILogger logger)
        {
            _logger = logger;   
        }
        public static void WriteRates(AppDbContext context, ExchangeRateData[] rates)
        {
            _logger.LogInformation("Ensuring data base is created: {time}", DateTimeOffset.Now);
            context.Database.EnsureCreated();

            _logger.LogInformation("Writing rates to data base: {time}", DateTimeOffset.Now);
            context.Rates.AddRange(rates);
            context.SaveChanges();
        }

        public static List<ExchangeRateData> ReadAllRates(AppDbContext context)
        {
            return context.Rates.ToList();
        }

        public static void ClearTable(AppDbContext context)
        {
            _logger.LogInformation("Ensuring data base is created: {time}", DateTimeOffset.Now);

            context.Database.EnsureCreated();

            _logger.LogInformation("Clearing table: {time}", DateTimeOffset.Now);

            context.Database.ExecuteSqlRaw($"TRUNCATE TABLE [{AppDbContext.TABLE_NAME}]");
        }

        public static void CleanupRates(AppDbContext context, bool cleanupMainRates)
        {
            string rates = cleanupMainRates ? "'Main'" : "'Other'"; 
            _logger.LogInformation($"Removing {cleanupMainRates} rates from data base: {DateTimeOffset.Now}");

            ExchangeRateData[] oldMainRates = context.Rates.Where(rate => rate.IsMain == cleanupMainRates).ToArray();
            context.Rates.RemoveRange(oldMainRates);
            context.SaveChanges();
        }
    }
}
