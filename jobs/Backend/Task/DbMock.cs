#nullable enable

using System;
using System.Collections.Generic;
using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater;

/**
 * Simulates a real data store of some kind, perhaps an SQL instance
 */

public class DbMock
{
    private static readonly Dictionary<DateOnly, List<ExchangeRate>> Cache = new();

    /**
     * Would be a database call to see if we have already persisted rates for the given date
     */
    public static List<ExchangeRate>? FindExchangeRates(DateOnly forDate)
    {
        return Cache.GetValueOrDefault(forDate);
    }

    public static void Insert(DateOnly forDate, List<ExchangeRate> newRates)
    {
        Cache[forDate] = newRates;
    }
}

