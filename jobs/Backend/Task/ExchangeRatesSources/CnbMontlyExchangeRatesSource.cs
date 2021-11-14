using System;

namespace ExchangeRateUpdater
{
    public class CnbMontlyExchangeRatesSource : CnbExchangeRatesSource
    {
        protected override string GetUrl(DateTime day)
        {
            return $"https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-ostatnich-men/kurzy-ostatnich-men/kurzy.txt?rok={day.Year}&mesic={day.Month}";
        }

        protected override bool IsOudated()
        {
            // https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-ostatnich-men/kurzy-ostatnich-men/
            // not sure if this is right
            return DownloadTime.Month != DateTimeProvider.Today.Month
                || DownloadTime.Year != DateTimeProvider.Today.Year;
        }
    }
}