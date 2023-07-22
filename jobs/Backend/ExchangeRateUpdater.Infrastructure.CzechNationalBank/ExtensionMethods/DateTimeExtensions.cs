namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.ExtensionMethods;

public static class DateTimeExtensions
{
    public static string ToCzechNationalBankExchangeNow(this DateTime datetime)
    {
        /*
         * As Per https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/
         * Exchange rates of commonly traded currencies are declared every working day after 2.30 p.m.
         * and are valid for the current working day and, where relevant, the following Saturday,
         * Sunday or public holiday (for example, an exchange rate declared on Tuesday 23 December is valid
         * for Tuesday 23 December, the public holidays 24–26 December, and Saturday 27 December and Sunday 28 December).
         */
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        var pragueLocalTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);
        var hour = pragueLocalTime.Hour;
        var minute = pragueLocalTime.Minute;

        if (hour <= 14 && minute < 30)
        {
            return pragueLocalTime.AddDays(-1).ToString("yyyy-MM-dd");
        }
        return pragueLocalTime.ToString("yyyy-MM-dd");
    }    
    
    public static string ToOtherCurrenciesExchangeNow(this DateTime datetime)
    {
        /*
         * As per https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/
         * The exchange rates of other currencies are declared on the last working day of the month
         * and are valid for the entire following month (for example, an exchange rate declared
         * on Friday 26 February is valid for each day between 1 March and 31 March).
         */
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        var pragueLocalTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);
        return pragueLocalTime.AddMonths(-1).ToString("yyyy-MM");
    }
}