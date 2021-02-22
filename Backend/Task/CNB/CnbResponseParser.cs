using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.CNB
{
    public class CnbResponseParser : ICnbResponseParser
    {
        private readonly Lazy<XmlSerializer> serializer = 
            new Lazy<XmlSerializer>(() =>
                new XmlSerializer(typeof(ExchangeRatesCollectionDto)));

        public ExchangeRatesCollectionDto Parse(Stream content)
        {
            return (ExchangeRatesCollectionDto)(serializer.Value)
                .Deserialize(content);
        }
    }

    public interface ICnbResponseParser
    {
        ExchangeRatesCollectionDto Parse(Stream content);
    }
}
