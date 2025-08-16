using ExchangeRateUpdater.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit.CacheTests;

internal class ReferenceTimeTestDouble : ReferenceTime
{
    private DateTime _time = DateTime.Now;

    public override DateTime GetTime() => _time;

    public void SetTime(DateTime newTime)
    {
        _time = newTime;
    }
}
