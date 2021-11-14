using System;

namespace ExchangeRateUpdater
{
    public class CnbDailyExchangeRatesSource : CnbExchangeRatesSource
    {
        protected override string GetUrl(DateTime day)
        {
            return $"https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt?date={day.Day}.{day.Month}.{day.Year}";
        }

        protected override bool IsOudated()
        {
            // https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/
            // not sure if this is right
            if (DateTimeProvider.Now < DateTimeProvider.Today.AddHours(14).AddMinutes(30))
                return DownloadTime.Date != DateTimeProvider.Today.Date;
            else
                return DownloadTime < DateTimeProvider.Today.AddHours(14).AddMinutes(30);
        }
    }
}