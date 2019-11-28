using System;

namespace ExchangeRateUpdater.CnbProvider
{
    /// <summary>
    ///  The rates of other currencies are set every last working day in a month.
    ///  https://www.cnb.cz/en/financial-markets/foreign-exchange-market/methodology-rates-of-other-currencies/
    /// </summary>
    class CnbFxOtherCurrenciesSource : CnbFxRatesSource
    {
        //Time is not clear, lets assume its same as for main currencies
        private static readonly (int hour, int minutes) PlannedFxRateUpdateTime = (hour: 14, minutes: 30);

        private readonly Func<DateTime> _dateTimeNowFunc;

        public CnbFxOtherCurrenciesSource() : this(() => DateTime.Now) 
        { }

        internal CnbFxOtherCurrenciesSource(Func<DateTime> dateTimeNowFunc)
        {
            _dateTimeNowFunc = dateTimeNowFunc ?? throw new ArgumentNullException(nameof(dateTimeNowFunc));
        }

        public override string Url => "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";

     
        public override string CreateQueryString()
        {
            var dtNow = _dateTimeNowFunc();
            var lastDayOfTheMonth = new DateTime(dtNow.Year, dtNow.Month, DateTime.DaysInMonth(dtNow.Year, dtNow.Month), PlannedFxRateUpdateTime.hour, PlannedFxRateUpdateTime.minutes, 0);

            var scheduledDateTimeAtTheEndOfTheMonth = base.AdjustDateToLastWorkingDay(lastDayOfTheMonth);

            //if new rates where not published yet this month, we need to get previous ones. No need for precises day 
            if (scheduledDateTimeAtTheEndOfTheMonth > dtNow) 
                scheduledDateTimeAtTheEndOfTheMonth = scheduledDateTimeAtTheEndOfTheMonth.AddMonths(-1);
            
            return $"year={scheduledDateTimeAtTheEndOfTheMonth.Year}&month={scheduledDateTimeAtTheEndOfTheMonth.Month}";
        }
    }
}