using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models
{
    public class CnbResponseTable
    {
        [XmlElement("radek")]
        public List<CnbResponseTableRow> Rows { get; set; }
    }
}