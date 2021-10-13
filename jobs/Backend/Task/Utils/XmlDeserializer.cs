using System;
using System.IO;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Utils
{
    public class XmlDeserializer
    {

        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static T DeserializeXMLFileToObject<T>(string xmlFile)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(xmlFile)) return default(T);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                returnObject = (T)serializer.Deserialize(new StringReader(xmlFile));
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, $"XML is not valid, please check element and attribute names, see inner exception: ${ex.InnerException}");

            }
            return returnObject;
        }
    }
}
