using ExchangeRateUpdater.CnbProvider.Const;
using ExchangeRateUpdater.CnbProvider.Enums;
using System;

namespace ExchangeRateUpdater.CnbProvider
{
    public class CnbRateRequestVO
    {
        private readonly DateTime _date;
        private readonly string _language;

        public CnbRateRequestVO(DateTime date,CnbLanguageEnum language)
        {
            _date = date;
            _language = language.ToString();
        }

        public string UrlDaily()
        {
            return string.Concat($"{CnbUrlConst.DailyUrl}?date={_date.ToString("yyyy-MM-dd")}&lang={_language}");
        }

        public string UrlMonthly()
        {
            return string.Concat($"{CnbUrlConst.MonthlyUrl}?yearMonth={_date.AddMonths(-1).ToString("yyyy-MM")}&lang={_language}");
        }

    }
}
