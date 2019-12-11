using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class CurrentRatesData
    {
        public CurrentRatesData()
        {
            Date = DateTime.MinValue;
            Currencies = new SortedDictionary<string, decimal>();
        }

        public DateTime Date { get; set; }
        public SortedDictionary<string, decimal> Currencies { get; set; }

        public void UpdateDate(string strDate)
        {
            var newDate = DateTime.Parse(strDate);
            if (newDate <= Date) return;
            Date = newDate;
        }
    }
}