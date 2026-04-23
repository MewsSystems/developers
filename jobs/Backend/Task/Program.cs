using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeService;
using Logger;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class Program
    {
        public static async Task Main()
        {
            ILogger logger = new LoggerService().logger;
            ExchangeRateService exchangeRateService = new ExchangeRateService(logger);

            try
            {
                // App entry point should not care about business logic.
                // Using an Execute service, we can swap out BL and APIs etc
                await exchangeRateService.Execute();
            }
            catch (Exception ex)
            {
                logger.LogError($"Program encountered an unhandled exception: {ex.Message}");

                // Don't advertise our stack trace and error to the front end user
                Console.WriteLine($"Failed to retrieve Exchange Rates. Please try again.");
            }

            Console.ReadLine();
        }
    }
}
