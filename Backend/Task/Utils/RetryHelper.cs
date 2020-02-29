using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Utils
{
    internal class RetryHelper
    {
        public static async Task<T> RetryOnExceptionAsync<T>(
         Func<Task<T>> operation,
         int times,
         TimeSpan delay,
         ILogger logger)
        {
            var attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    return await operation();
                }
                catch (Exception ex)
                {
                    if (attempts == times)
                    {
                        logger.LogError($"Exception on attempt {attempts} of {times} - throwing. ({ex.Message})");
                        throw;
                    }
                    await CreateDelayForException(logger, times, attempts, delay, ex);
                }
            } while (true);
        }

        private static Task CreateDelayForException(ILogger logger, int times, int attempts, TimeSpan delay, Exception ex)
        {
            string message = $"Exception on attempt {attempts} of {times}. Will retry after sleeping for {delay}. ({ex.Message})";
            logger.LogWarning(message);
            return Task.Delay(delay);
        }
    }
}
