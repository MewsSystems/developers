using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.CnbProvider
{
    abstract class CnbFxRatesSource
    {
        protected CnbFxRatesSource()
        {
            Current = new Context(new List<ExchangeRate>(), string.Empty, string.Empty);
        }

        public Context Current { get; set; }

        /// <summary>
        /// Endpoint url
        /// </summary>
        public abstract string Url { get; }

        /// <summary>
        /// Generate a query string for given API
        /// </summary>
        public abstract string CreateQueryString();

        /// <summary>
        /// True if we expect that new rates where published for that source
        /// </summary>
        public virtual bool NewRatesAvailable()
        {
           return Current.LastUsedQueryString == string.Empty || Current.LastUsedQueryString != CreateQueryString();
        }

        /// <summary>
        /// Has document changed since last update
        /// </summary>
        public virtual bool VersionsMatch(string topDocumentRow)
        {
            return topDocumentRow == Current.VersionRowFromPrevUpdate;
        }

        public class Context
        {
            public Context(IEnumerable<ExchangeRate> newRates, string usedQueryString, string versionRow)
            {
                LoadedRates = newRates;
                LastUsedQueryString = usedQueryString;
                VersionRowFromPrevUpdate = versionRow;
            }

            public IEnumerable<ExchangeRate> LoadedRates { get; }
            public string LastUsedQueryString { get; }
            public string VersionRowFromPrevUpdate { get; }
        }

        protected DateTime AdjustDateToLastWorkingDay(DateTime fromDateTime)
        {
            switch (fromDateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return fromDateTime.AddDays(-2);
                case DayOfWeek.Saturday:
                    return fromDateTime.AddDays(-1);
                default:
                    //Bug: There will be no update published on public holidays.
                    //Please, don't run this code on prod during czech public holidays.
                    return fromDateTime;
            }
        }
    }
}