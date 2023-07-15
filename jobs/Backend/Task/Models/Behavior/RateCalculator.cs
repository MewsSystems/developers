using ExchangeRateUpdater.Models.Types;
using System;

namespace ExchangeRateUpdater.Models.Behavior
{
    internal static class RateCalculator
    {
        internal static Rate GetByAmount(this Rate rate, int amount) =>
            new(Math.Round(rate.Value / amount, 2));
    }
}
