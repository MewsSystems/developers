using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    public class CnbTable
    {
        [XmlElement("radek")]
        public List<CnbRow> rows;
    }
}
