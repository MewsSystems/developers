using System.IO;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    public static class XmlHelper
    {
        public static T Deserialize<T>(string value)
        {
            using (TextReader reader = new StringReader(value))
            {               
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
        }
    }
}
