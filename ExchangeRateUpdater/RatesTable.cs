using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    [DataContract]
    public class RatesTableDto
    {
        [DataMember(Name = "table", IsRequired = true)]
        public char TableName { get; set; }
        [DataMember(Name = "no")]
        public string TableNumber { get; set; }
        [DataMember(Name = "effectiveDate")]
        public string EffectiveDate { get; set; }
        [DataMember(Name = "rates", IsRequired = true)]
        public Rate[] Rates { get; set; }

    }

    [DataContract]
    public class Rate
    {
        [DataMember(Name = "currency", IsRequired = true)]
        public string Currency { get; set; }
        [DataMember(Name = "code", IsRequired = true)]
        public string Code { get; set; }
        [DataMember(Name = "mid", IsRequired = true)]
        public decimal Value { get; set; }

    }
}

