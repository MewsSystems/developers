/*
   System:         N/A
   Component:      ExchangeRateUpdater
   Filename:       Schedules.cs
   Created:        01/08/2023
   Original Author:Tom Doran
   Purpose:        Class for building schedules for the Exchange Rate cache.
 */

using Quartz.Impl;
using Quartz;
using System;

namespace ExchangeRateUpdater.Scheduler
{
    internal static class Schedules
    {
        private static IScheduler _scheduler;
        internal static void SetupSchedules()
        {
            // Grab the Scheduler instance from the Factory & start it
            var factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler().Result;
            _scheduler.Start();

            var triggerTime = SetTriggerTime();
            // setup schedule for updating the cache                          
            IJobDetail jobDetail = JobBuilder.Create<CacheUpdateJob>()
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartAt(triggerTime)
                .Build();

            _scheduler.ScheduleJob(jobDetail, trigger);
            Console.WriteLine($"Cache Update Job scheduled for {triggerTime.ToString("HH.mm.ss.fff")}");
        }

        internal static void StopSchedules()
        {
            _scheduler.Shutdown();
        }

        /// <summary>
        /// Set the trigger time for the cache update schedule.
        /// This would ideally be set from config instead of using hardcoded values.
        /// </summary>
        /// <returns></returns>
        private static DateTime SetTriggerTime()
        {
            //  Realistic value:        (2:30PM Czech time)
            /*
            var twoThirtyCzechTime = new TimeSpan(15, 30, 00);   // czech time is 1 hr ahead
            var triggerTime = DateTime.Now.Date + twoThirtyCzechTime;         
            return triggerTime;                
            */

            // For testing/demo:         (1 minute from now)
            var triggerTime = DateTime.Now;
            triggerTime = triggerTime.Add(TimeSpan.FromMinutes(1));
            return triggerTime;
        }
    }
}