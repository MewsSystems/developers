using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Entities;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Infrastructure
{
    public interface IExchangeRateMapper
    {
        Result<ExchangeRate[]> Map(ReadOnlySpan<char> data);
    }
}