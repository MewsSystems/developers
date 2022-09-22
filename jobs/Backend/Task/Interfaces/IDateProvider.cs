using System;

namespace ExchangeRateUpdater.Interfaces;

public interface IDateProvider
{
    public DateOnly ForToday();
}