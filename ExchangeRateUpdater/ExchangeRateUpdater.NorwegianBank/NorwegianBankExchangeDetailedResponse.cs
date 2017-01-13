using ExchangeRateUpdater.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.NorwegianBank
{
    //{"Updated":"2017-01-12T16:02:11Z","TableNameHeader":"Currency (code)","TableGraphHeader":"Last mth","TableDynamicHeaders":["12 Jan","11 Jan"],"TableEntries":[{"Name":"Indonesian rupiah (IDR)","Id":"IDR","ConversionFactor":100,"Values":["0.064043","0.064740"],"GraphUrl":"/WebDAV/stat/Valutakurser/png/idr.png"},{"Name":"US dollar (USD)","Id":"USD","ConversionFactor":1,"Values":["8.4863","8.6292"],"GraphUrl":"/WebDAV/stat/Valutakurser/png/usd.png"}]}
    class NorwegianBankExchangeDetailedResponse
    {
        public IEnumerable<NorwegianBankExchangeRate> TableEntries { get; set; }
        public DateTime Updated { get; set; }
        public string TableNameHeader { get; set; }
        public string TableGraphHeader { get; set; }        
    }

    class NorwegianBankExchangeRate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GraphUrl { get; set; }
        public int ConversionFactor { get; set; }
        public decimal[] Values { get; set; }

        public ExchangeRate ToExchangeRate()
        {
            if (string.IsNullOrWhiteSpace(this.Id) || ConversionFactor == 0 || Values == null || !Values.Any())
                throw new ArgumentNullException("NorwegianBankExchangeResponse");

            return new ExchangeRate(new Currency("NOK"), new Currency(this.Id), this.Values[0]);
        }
    }
}
