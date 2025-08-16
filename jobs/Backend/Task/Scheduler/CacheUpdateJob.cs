/*
   System:         N/A
   Component:      ExchangeRateUpdater
   Filename:       CacheUpdateJob.cs
   Created:        01/08/2023
   Original Author:Tom Doran
   Purpose:        Scheduled job for updating the exchange rate cache
 */

using Quartz;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Scheduler
{
    internal class CacheUpdateJob : IJob
    {
        /// <summary>
        /// Job to refresh cache at the configured time.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            await ExchangeRateCache.RefreshCache();
        }
    }
}
