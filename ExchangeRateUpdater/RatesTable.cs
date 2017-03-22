using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    [DataContract]
    public class RatesTable
    {
        [DataMember]
        public char table { get; set; }
        [DataMember]
        public string no { get; set; }
        [DataMember]
        public string effectiveDate { get; set; }
        [DataMember]
        public Rate[] rates { get; set; }

    }

    [DataContract]
    public class Rate
    {
        [DataMember]
        public string currency { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public decimal mid { get; set; }

    }
}

