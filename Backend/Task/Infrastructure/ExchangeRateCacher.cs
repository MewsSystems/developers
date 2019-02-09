using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure
{


    /// <summary>
    /// This is cache for Exchange Rates. Getting it from the bank is a bit time consuming so it is better to be cached. 
    /// Bank releases exchange rates once per day. Request is supposed to be synchronous, but no more than one network call is required.
    /// </summary>
    public class ExchangeRateCacher : IDisposable
    {
        #region --- Singleton ---

        private ExchangeRateCacher()
        {

        }

        private static ExchangeRateCacher instance;

        public static ExchangeRateCacher Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ExchangeRateCacher();
                    instance.LoadCache();
                }

                return instance;
            }
        }

        #endregion

        #region private properties

        private ExchangeRatesCache CachedValues { get; set; }
        private Task LoadTask;
        private object lockObject = new object();  
        
        #endregion

        #region setup constants

        public const string EXCHANGE_SOURCE_NEUTRAL = @"http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";
        public const string EXCHANGE_SOURCE_URL = @"http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt?date=";
        public const string DATE_TIME_FORMAT = @"dd.MM.yyyy";
        public const string DEFAULT_NATIVE_CURRENCY = @"CZK";
        public const string DATE_TIME_PATTERN = @"^\d{2}\.\d{2}\.\d{4}";
        public const string CURRENCY_PATTERN = @"\d+\|\w{3}\|\d+\S\d+";    
        
        #endregion


        #region download & parse

        private async Task DownloadExchangeRates(DateTime? loadDate, string homeCurrency)
        {
            string callUrl = loadDate == null
                ? EXCHANGE_SOURCE_NEUTRAL :
                EXCHANGE_SOURCE_URL + loadDate.Value.ToString(DATE_TIME_FORMAT);

            string content;

            using (var client = new HttpClient())
            {
                content = await client.GetStringAsync(callUrl);
            }

            var ratesList = ParseContent(content, homeCurrency);
            if (ratesList == null || ratesList.Count == 0)
            {
                this.CachedValues = new ExchangeRatesCache()
                {
                    ErrorMessage = "Unable to retreive date",
                    ValidForDate = DateTime.Now.AddMinutes(5),
                };
            }

            var validityDate = ParseValidityDate(content);
            var rates = ratesList.ToDictionary(item => Tuple.Create(item.SourceCurrency.Code, item.TargetCurrency.Code), item => item);

            this.CachedValues = new ExchangeRatesCache()
            {
                Exchange = rates,
                ValidForDate = validityDate,
            };
            
        }


        private List<ExchangeRate> ParseContent(string content, string callCurrency)
        {
            var returnList = new List<ExchangeRate>();
            var matches = Regex.Matches(content, CURRENCY_PATTERN);
            var provider = new CultureInfo("cs-CZ");
            foreach (var match in matches)
            {
                try
                {
                    var line = match.ToString().Split('|');
                    int amount = int.Parse(line[0]);
                    string code = line[1];
                    decimal rate = decimal.Parse(line[2], provider) / amount;
                    var exchangeRate = new ExchangeRate(new Currency(callCurrency), new Currency(code),rate);
                    returnList.Add(exchangeRate);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return returnList;
        }

        private DateTime ParseValidityDate(string content)
        {
            try
            {
                var match = Regex.Match(content, DATE_TIME_PATTERN);
                var endOfDay = DateTime.ParseExact(match.ToString(), CURRENCY_PATTERN, CultureInfo.InvariantCulture).Date.AddHours(23).AddMinutes(59);

                //Bank hasn't released statement for today yet. Increasing validity for one more hour.
                if(DateTime.Now > endOfDay)
                {
                    return DateTime.Now.AddHours(1);
                }

                return endOfDay;
            }
            catch(Exception exception)
            {
                return DateTime.Now.AddHours(1);
            }
        }


        private void LoadCache()
        {
            lock (lockObject)
            {
                if (LoadTask == null || (CachedValues.ValidForDate < DateTime.Now && LoadTask.IsCompleted || LoadTask.IsFaulted))
                {
                    this.LoadTask = DownloadExchangeRates(null, DEFAULT_NATIVE_CURRENCY);
                }
            }           
        }


        #endregion

        #region Wroking functions

        public ExchangeRate GetExchangeRate(string to)      
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                return null;
            }

            return GetExchangeRate(DEFAULT_NATIVE_CURRENCY , to);
        }

        public ExchangeRate GetExchangeRate(string from, string to)
        {
            try
            {
                if (LoadTask == null || (CachedValues != null && CachedValues.ValidForDate < DateTime.Now && (LoadTask.IsCompleted || LoadTask.IsFaulted)))
                {
                    LoadCache();
                }
                LoadTask.Wait(10000);
                var searchTuple = Tuple.Create(from, to);
                if (CachedValues.Exchange == null || !CachedValues.Exchange.ContainsKey(searchTuple))
                {
                    return null;
                }
                return CachedValues.Exchange[searchTuple];
            }
            catch(Exception exception)
            {
                return null;
            }
        }

        #endregion

        #region Cleanup

        public void Dispose()
        {         
            if (this.LoadTask.IsCompleted || this.LoadTask.IsCanceled || this.LoadTask.IsFaulted)
            {
                this.LoadTask.Dispose();
            }
            this.LoadTask = null;
            this.CachedValues = null;
            instance = null;
        }

        public void DropCache()
        {
            this.CachedValues = null;
        }

        #endregion
    }
}
