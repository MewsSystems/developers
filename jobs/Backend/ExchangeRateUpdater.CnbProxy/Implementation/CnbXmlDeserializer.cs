using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.CnbProxy.Implementation
{
    internal class CnbXmlDeserializer : ICnbXmlDeserializer
    {
        public T Deserialize<T>(string xmlContent) where T : class
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent));

            var result = (T)xmlSerializer.Deserialize(stream);

            return result;
        }
    }
}
