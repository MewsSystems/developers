using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Model.Cnb.Rq
{
    public class CnbExchangeRatesRqModel
    {
        public CnbExchangeRatesRqModel(string language)
        {
            Lang = language;
        }
        public string Lang { get; set; }
    }
}
