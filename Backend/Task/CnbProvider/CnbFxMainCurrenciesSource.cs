using System;

namespace ExchangeRateUpdater.CnbProvider
{
    /// <summary>
    /// Main currencies with planned update time daily as per https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates
    /// </summary>
    class CnbFxMainCurrenciesSource : CnbFxRatesSource
    {
        private static readonly (int hour, int minutes) PlannedFxRateUpdateTime = (hour: 14, minutes: 30);

        private readonly Func<DateTime> _dateTimeNowFunc;

        public CnbFxMainCurrenciesSource() : this(() => DateTime.Now)
        { }

        internal CnbFxMainCurrenciesSource(Func<DateTime> dateTimeNowFunc)
        {
            _dateTimeNowFunc = dateTimeNowFunc ?? throw new ArgumentNullException(nameof(dateTimeNowFunc));
        }

        public override string Url => "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        public override string CreateQueryString()
        {
            return $"date={GetLastExpectedUpdateDateTimeForFxRate(PlannedFxRateUpdateTime):dd.MM.yyyy}";
        }

        internal DateTime GetLastExpectedUpdateDateTimeForFxRate((int hour, int minutes) plannedRateUpdateTime)
        {
            var dtNow = _dateTimeNowFunc();
            DateTime scheduledUpdateTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, plannedRateUpdateTime.hour, plannedRateUpdateTime.minutes, 0);

            //if update was not published yet we need to get one from prev work day
            if (scheduledUpdateTime > dtNow)
                scheduledUpdateTime = scheduledUpdateTime.AddDays(-1);

            scheduledUpdateTime = base.AdjustDateToLastWorkingDay(scheduledUpdateTime);

            return scheduledUpdateTime;
        }
    }
}