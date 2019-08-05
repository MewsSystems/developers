using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ExchangeRateUpdater
{
    public abstract class AbstractExchangeRateProvider
    {
        protected virtual Currency TargetCurrency { get; set; }
        protected virtual string ExchangeRateUri { get; set; }
        protected abstract ExchangeRateRecord ParseLine(string line);
        protected virtual IEnumerable<ExchangeRateRecord> ReadAllExchangeRates()
        {
            var exchangeRateRecords = new List<ExchangeRateRecord>();

            using (var reader = new StreamReader(new WebClient().OpenRead(ExchangeRateUri)))
            {
                while (!reader.EndOfStream)
                {
                    var currencyLine = reader.ReadLine();

                    try
                    {
                        exchangeRateRecords.Add(ParseLine(currencyLine));
                    }

                    catch {
                        //some logging could be here
                    }  
                }
            }

            return exchangeRateRecords;
        }
    }
}