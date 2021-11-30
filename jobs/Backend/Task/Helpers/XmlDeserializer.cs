using System.IO;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Helpers
{
    public static class XmlDeserializer
    {
        public static T XmlDeserializeFromString<T>(string objectData)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(objectData))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
