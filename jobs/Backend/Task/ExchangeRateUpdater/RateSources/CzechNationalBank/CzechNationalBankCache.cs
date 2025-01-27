using System;
using ExchangeRateUpdater.Cache;
using PublicHoliday;

namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public class CzechNationalBankRatesCache
{
    static readonly TimeZoneInfo CETTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Time");
    static readonly TimeOnly MainRatesRefreshTime = new TimeOnly(14, 30);
    
    private readonly CzechRepublicPublicHoliday _holidays;
    private readonly TimeProvider _timeProvider;

    public CzechNationalBankRatesCache(CzechRepublicPublicHoliday holidays, TimeProvider timeProvider)
    {
        _holidays = holidays;
        _timeProvider = timeProvider;
    }

    public DateTime? GetMainRateExpirationDate(DateOnly targetDate)
    {
        var now = _timeProvider.GetUtcNow();
        var targetDateTime = targetDate.ToDateTime(new(0, 0));
        throw new NotImplementedException();
    }
}