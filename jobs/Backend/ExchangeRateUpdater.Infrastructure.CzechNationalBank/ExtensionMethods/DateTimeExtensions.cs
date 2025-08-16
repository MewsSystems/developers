namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.ExtensionMethods;

public static class DateTimeExtensions
{
    /// <summary>
    /// Returns the date in the format that the Czech National Bank uses to retrieve daily exchange rates
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    public static string ToCzechNationalBankExchangeNowString(this DateTime datetime)
    {
        /*
         * As Per https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/
         * Exchange rates of commonly traded currencies are declared every working day after 2.30 p.m.
         * and are valid for the current working day and, where relevant, the following Saturday,
         * Sunday or public holiday (for example, an exchange rate declared on Tuesday 23 December is valid
         * for Tuesday 23 December, the public holidays 24–26 December, and Saturday 27 December and Sunday 28 December).
         */
        var pragueLocalTime = datetime.ToPragueLocalTime();
        var hour = pragueLocalTime.Hour;
        var minute = pragueLocalTime.Minute;

        if (hour <= 14 && minute < 30)
        {
            return pragueLocalTime.AddDays(-1).ToString("yyyy-MM-dd");
        }
        return pragueLocalTime.ToString("yyyy-MM-dd");
    }    
    
    /// <summary>
    /// Returns the date in the format that the Czech National Bank uses to retrieve monthly exchange rates for other currencies
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    public static string ToOtherCurrenciesExchangeNowString(this DateTime datetime)
    {
        /*
         * As per https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/
         * The exchange rates of other currencies are declared on the last working day of the month
         * and are valid for the entire following month (for example, an exchange rate declared
         * on Friday 26 February is valid for each day between 1 March and 31 March).
         */
       
        return datetime.ToPragueLocalTime().AddMonths(-1).ToString("yyyy-MM");
    }

    /// <summary>
    /// Returns the date in a format that can be used as a cache key reference to ensure cached values are not stale
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    public static string ToCacheKeyReferenceString(this DateTime datetime)
    {
        /*
         * To ensure we do not refer to stale values, cache key is worked out based on the current day and time,
         * ensuring daily rates do not go stale and and the current month (-1) to cater for other currencies.
         */
        var pragueLocalTimeForCache = datetime.ToPragueLocalTime().AddMonths(-1);
        var hour = pragueLocalTimeForCache.Hour;
        var minute = pragueLocalTimeForCache.Minute;

        if (hour <= 14 && minute < 30)
        {
            return pragueLocalTimeForCache.AddDays(-1).ToString("yyyy-MM-dd");
        }
        return pragueLocalTimeForCache.ToString("yyyy-MM-dd");
    }

    private static DateTime ToPragueLocalTime(this DateTime datetime)
    {
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        return TimeZoneInfo.ConvertTime(datetime, timeZoneInfo);
    }
}