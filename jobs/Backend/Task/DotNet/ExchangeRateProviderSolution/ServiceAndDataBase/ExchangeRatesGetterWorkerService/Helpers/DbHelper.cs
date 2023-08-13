
using ExchangeRatesGetterWorkerService.Context;
using ExchangeRatesGetterWorkerService.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesGetterWorkerService.Helpers
{

    /// <summary>   A database helper. </summary>
    ///
    public class DbHelper
    {
        /// <summary>   The logger. </summary>
        private static ILogger _logger;

    
        /// <summary>   Initializes this object. </summary>
        ///
        ///
        /// <param name="logger">   The logger. </param>
        public static void Init(ILogger logger)
        {
            _logger = logger;   
        }

    
        /// <summary>   Writes the rates. </summary>
        ///
        ///
        /// <param name="context">  The context. </param>
        /// <param name="rates">    The rates. </param>
        public static void WriteRates(AppDbContext context, ExchangeRateData[] rates)
        {
            _logger.LogInformation("Ensuring data base is created: {time}", DateTimeOffset.Now);
            context.Database.EnsureCreated();

            _logger.LogInformation("Writing rates to data base: {time}", DateTimeOffset.Now);
            context.Rates.AddRange(rates);
            context.SaveChanges();
        }

    
        /// <summary>   Reads all rates. </summary>
        ///
        ///
        /// <param name="context">  The context. </param>
        ///
        /// <returns>   all rates. </returns>
        public static List<ExchangeRateData> ReadAllRates(AppDbContext context)
        {
            return context.Rates.ToList();
        }

    
        /// <summary>   Clears the table described by context. </summary>
        ///
        ///
        /// <param name="context">  The context. </param>
        public static void ClearTable(AppDbContext context)
        {
            _logger.LogInformation("Ensuring data base is created: {time}", DateTimeOffset.Now);

            context.Database.EnsureCreated();

            _logger.LogInformation("Clearing table: {time}", DateTimeOffset.Now);

            context.Database.ExecuteSqlRaw($"TRUNCATE TABLE [{AppDbContext.TABLE_NAME}]");
        }

    
        /// <summary>   Cleanup rates. </summary>
        ///
        ///
        /// <param name="context">          The context. </param>
        /// <param name="cleanupMainRates"> True to cleanup main rates. </param>
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
