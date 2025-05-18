using System;

namespace ExchangeRateUpdater.Infrastructure.CNB
{
    internal record CNBOptions
    {
        public string BaseUrl { get; set; }

        public TimeOnly NewDataSchedule { get; set; }
    }
}
