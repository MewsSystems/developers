using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.CNB
{
    [Serializable()]
    [XmlRoot("kurzy")]
    public class ExchangeRatesCollectionDto
    {
        [XmlArray("tabulka")]
        [XmlArrayItem("radek", typeof(ExchangeRateDto))]
        public ExchangeRateDto[] Table { get; set; }

    }
}
