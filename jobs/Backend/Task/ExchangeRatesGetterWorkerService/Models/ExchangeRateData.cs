using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRatesGetterWorkerService.Models
{
    public class ExchangeRateData
    {
        public int Id { get; set; }
        public String SourceCurrency { get; set; }

        public String TargetCurrency { get; set; }

        public decimal Value { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }

    }
}
