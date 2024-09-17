using Mews.ExchangeRate.Updater.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mews.ExchangeRate.Updater.Services.Abstractions
{
    public class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
