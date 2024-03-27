using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider.DomainEntities
{
    public class RequestModel
    {
        public DateTime DateTime { get; set; }
        public string Language { get; set; }
    }
}
